﻿using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RafaStore.Server.Services.FileService;
using RafaStore.Server.Util;
using RafaStore.Shared;
using RafaStore.Shared.Model;
using RafaStore.Shared.ViewModel;
using System.Globalization;

namespace RafaStore.Server.Services.HospitalService
{
    public class CustomerService : ICustomerService
    {
        private readonly CultureInfo pt = new("pt-BR");
        private readonly RegionInfo region = new("pt-BR");
        private readonly DataContext context;
        private readonly IFileService _fileService;
        public CustomerService(DataContext context, IFileService fileService)
        {
            this.context = context;
            _fileService = fileService;
        }

        public async Task<ServiceResponse<CustomerModel>> Add(CustomerModel customer)
        {
            var customerExists = await CustomerExistsByName(customer.Name);

            if (customerExists is not null)
            {
                return new ServiceResponse<CustomerModel>
                {
                    Success = false,
                    Message = "O cliente ja existe"
                };
            }

            if (customer.CpfOrCnpj.Length == 11)
                customer.CpfOrCnpj = HelperCpfOrCnpj.FormatCPF(customer.CpfOrCnpj);
            else
                customer.CpfOrCnpj = HelperCpfOrCnpj.FormatCNPJ(customer.CpfOrCnpj);

            context.Customer.Add(customer);
            await context.SaveChangesAsync();

            return new ServiceResponse<CustomerModel> { Data = customer, Message = "Cliente cadastrado com sucesso!" };
        }

        public async Task<ServiceResponse<CustomerModel>> Update(CustomerModel customer)
        {
            var result = await CustomerExistsById(customer.Id);

            if (result is null)
                return new ServiceResponse<CustomerModel>
                {
                    Success = false,
                    Message = "O customer selecionado nao existe.",
                };

            result.Name = customer.Name;
            result.Address = customer.Address;

            var semFormatacao = HelperCpfOrCnpj.SemFormatacao(customer.CpfOrCnpj);

            if (semFormatacao.Length == 11)
                result.CpfOrCnpj = HelperCpfOrCnpj.FormatCPF(semFormatacao);
            else
                result.CpfOrCnpj = HelperCpfOrCnpj.FormatCNPJ(semFormatacao);

            context.Customer.Update(result);
            await context.SaveChangesAsync();

            return new ServiceResponse<CustomerModel>
            {
                Data = result,
                Message = "Customer atualizado com sucesso.",
            };
        }

        private async Task<CustomerModel> CustomerExistsByName(string name)
        {
            return await context.Customer.FirstOrDefaultAsync(customer => customer.Name.ToLower().Equals(name.ToLower()));
        }

        private async Task<CustomerModel> CustomerExistsById(int? id)
        {
            return await context.Customer.FirstOrDefaultAsync(customer => customer.Id == id);
        }

        public async Task<ServiceResponse<CustomerListViewModel>> SearchCustomers(string searchText, int page)
        {
            if (string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText))
                return await GetAllCustomersPaginated(page);

            var customersFound = await FindCustomersBySearchText(searchText);

            return await PaginateCustomers(customersFound, page);
        }

        private async Task<ServiceResponse<CustomerListViewModel>> PaginateCustomers(List<CustomerModel> customer, int page)
        {
            var pageResults = 5f;
            var pageCount = Math.Ceiling((customer).Count / pageResults);

            var paginatedCustomers = customer.Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();

            return new ServiceResponse<CustomerListViewModel>
            {
                Data = new CustomerListViewModel
                {
                    Customers = paginatedCustomers,
                    CurrentPage = page,
                    Pages = (int)pageCount,
                }
            };
        }

        public async Task<ServiceResponse<CustomerModel>> GetCustomerById(int id)
        {
            var result = await CustomerExistsById(id);

            if (result is null)
                return new ServiceResponse<CustomerModel>
                {
                    Data = null,
                    Success = false,
                    Message = "O cliente nao existe."
                };

            return new ServiceResponse<CustomerModel> { Data = result, Message = "Cliente encontrado com sucesso." };
        }

        private async Task<List<CustomerModel>> FindCustomersBySearchText(string searchText)
        {
            return await context.Customer
                .Where(r => r.Name.ToLower().Contains(searchText.ToLower()))
                .ToListAsync();
        }

        public async Task<ServiceResponse<CustomerListViewModel>> GetAllCustomersPaginated(int page)
        {
            var customer = await context.Customer.ToListAsync();

            return await PaginateCustomers(customer, page);
        }

        public async Task<byte[]> GeneratePdf(GeneratePdfViewModel customer)
        {
            Thread.CurrentThread.CurrentCulture = pt;
            var pdf = Document.Create(document =>
                     {
                         document.Page(page =>
                         {
                             page.Size(PageSizes.A4);

                             page.Content().MinimalBox().PaddingTop(30).PaddingHorizontal(10).Border(1).Column(x =>
                             {
                                 for (int i = 0; i < customer.Note.NumeroDeParcelas; i++)
                                 {
                                     var stringDate = DateTime.Now.Date.AddMonths(i + 1);
                                     x.Spacing(2);
                                     x.Item().Element(x => ComposeHeader(x, stringDate));
                                     x.Item().PaddingRight(5).Element(y => ComposeBody(y, customer.Note, i));
                                     x.Item().PaddingLeft(5).Element(z => ComposeFooter(z, customer, stringDate));
                                     x.Item().PaddingTop(0).PaddingLeft(5).Element(i => ComposeParcialBody(i, customer.Customer, stringDate));
                                     if (i < customer.Note.NumeroDeParcelas - 1)
                                         x.Item().LineHorizontal(5);
                                 }
                             });
                         });
                     })
                     .GeneratePdf();

            await _fileService.CreateFile(new MemoryStream(pdf), customer.Customer, new NoteFileModel
            {
                FileName = $"{customer.Customer.Name}-{DateTime.Now:dd-MM-yyyy-HH-mm-ss}",
                CustomerModelId = customer.Customer.Id,
                ValorParcela = Math.Round((decimal)(customer.Note.ValorTotal / customer.Note.NumeroDeParcelas), 2),
                ValorTotal = customer.Note.ValorTotal,
                NumeroDeParcelas = customer.Note.NumeroDeParcelas
            }) ;

            return pdf;
        }

        public async Task DeleteCustomer(int customerId)
        {
            var customer = await context.Customer.Include(x => x.File).SingleOrDefaultAsync(x => x.Id == customerId);

            context.Customer.Remove(customer);

            await context.SaveChangesAsync();

            foreach (var file in customer.File)
            {
                await _fileService.DeleteFile(file.Id);
            }
        }

        public async Task<byte[]> DownloadCustomerNote(int noteId)
        {
            var note = await _fileService.GetNoteById(noteId);

            return await _fileService.DownloadToStream(note.Blob, note.FileName);
        }

        private void ComposeHeader(IContainer container, DateTime date)
        {
            container.Row(row =>
            {
                row.RelativeItem().PaddingTop(1).AlignRight().PaddingRight(5).Text($"Vencimento: {date:dd/MM/yyyy}".ToUpper()).FontColor("#F00");
            });
        }


        private void ComposeBody(IContainer container, NoteViewModel note, int i)
        {
            container.Row(row =>
            {
                row.ConstantItem(250);
                row.RelativeItem().Border(1).AlignCenter().Text($"  N {i + 1}" + "/" + $"{note.NumeroDeParcelas}  ").FontColor("#F00");
                row.ConstantItem(110);
                row.RelativeItem().Border(1).AlignCenter().Text($"R$ {Math.Round((decimal)(note.ValorTotal / note.NumeroDeParcelas), 2)}").FontColor("#F00");
            });
        }

        private void ComposeFooter(IContainer container, GeneratePdfViewModel customer, DateTime date)
        {
            var valorParcela = Math.Round(Convert.ToDecimal(customer.Note.ValorTotal / customer.Note.NumeroDeParcelas), 2);

            container.Row(row =>
            {
                row.RelativeItem().AlignLeft().PaddingRight(0).Text(text =>
                {
                    text.Span("Ao(s)".ToUpper());
                    text.Span("   ");
                    text.Span($"{pt.DateTimeFormat.GetDayName(date.DayOfWeek)}, {date.Day} de {pt.DateTimeFormat.GetMonthName(date.Month)} de {date.Year} pagarei por esta única via de nota promissória".ToUpper()).FontSize(10).Underline();
                    text.Span("  a  ".ToUpper());
                    text.Span("BETTINARDI ROUPAS E ACESSORIOS LTDA".ToUpper()).FontSize(10).Bold().Underline();
                    text.Span("  CPF/CNPJ:  ");
                    text.Span($"33.761.757/0001-57").Underline();
                    text.Span("  ou à sua ordem, a quantia de".ToUpper());
                    text.Span($"  R$ {valorParcela} {HelperNumberToText.EscreverExtenso(valorParcela)}".ToUpper()).FontSize(10).Bold();
                    text.Span("  em moeda corrente, pagável em LONDRINA - PARANÁ".ToUpper()).FontSize(10);
                    text.AlignLeft();
                });
            });
        }

        private void ComposeParcialBody(IContainer container, CustomerModel customer, DateTime date)
        {
            container.Row(row =>
            {
                row.RelativeItem().PaddingTop(5).PaddingBottom(0).Text(text =>
                {
                    text.Line($"Data de emissão: {DateTime.Now:dd/MM/yyyy}".ToUpper()).FontSize(10).FontColor("#F00");
                    text.Line($"Emitente: {customer.Name}".ToUpper()).FontSize(10);
                    text.Line($"CPF/CNPJ: {customer.CpfOrCnpj}".ToUpper()).FontSize(10);
                    text.Line($"Endereço: {customer.Address}".ToUpper()).FontSize(10);
                });
                row.ConstantItem(50);
                row.RelativeItem().PaddingRight(5).PaddingBottom(0).Text(text =>
                {
                    text.EmptyLine();
                    text.Line("                                                                                                  ").Underline();
                    text.Line($"{customer.Name}".ToUpper()).FontSize(10).Bold();
                    text.Line($"{customer.CpfOrCnpj}".ToUpper()).FontSize(10).Bold();
                    text.AlignLeft();
                });
            });
        }
    }
}
