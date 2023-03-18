using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
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
        public CustomerService(DataContext context)
        {
            this.context = context;
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

        public byte[] GeneratePdf(GeneratePdfViewModel customer)
        {
            Thread.CurrentThread.CurrentCulture = pt;
            var pdf = Document.Create(document =>
                     {
                         document.Page(page =>
                         {
                             page.Size(PageSizes.A4);

                             page.Content().MinimalBox().PaddingVertical(2).PaddingHorizontal(10).Border(1).Column(x =>
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

            return pdf;
        }

        private void ComposeHeader(IContainer container, DateTime date)
        {
            container.Row(row =>
            {
                row.RelativeItem().PaddingTop(1).AlignRight().PaddingRight(5).Text($"Vencimento: {date:dd/MM/yyyy}");
            });
        }


        private void ComposeBody(IContainer container, NoteViewModel note, int i)
        {
            container.Row(row =>
            {
                row.ConstantItem(250);
                row.RelativeItem().Border(1).AlignCenter().Text($"  N {i + 1}" + "/" + $"{note.NumeroDeParcelas}  ");
                row.ConstantItem(110);
                row.RelativeItem().Border(1).AlignCenter().Text($"R$ {Math.Round((decimal)(note.ValorTotal / note.NumeroDeParcelas), 2)}");
            });
        }

        private void ComposeFooter(IContainer container, GeneratePdfViewModel customer, DateTime date)
        {
            var valorParcela = Math.Round(Convert.ToDecimal(customer.Note.ValorTotal / customer.Note.NumeroDeParcelas), 2);

            container.Row(row =>
            {
                row.RelativeItem().AlignLeft().PaddingRight(0).Text(text =>
                {
                    text.Span("Ao(s)");
                    text.Span("   ");
                    text.Span($"{pt.DateTimeFormat.GetDayName(date.DayOfWeek)}, {date.Day} de {pt.DateTimeFormat.GetMonthName(date.Month)} de {date.Year} pagarei por esta única via de nota promissória").Underline();
                    text.Span("  a  ");
                    text.Span("BETTINARDI ROUPAS E ACESSORIOS LTDA").Bold().Underline();
                    text.Span("  CPF/CNPJ:  ");
                    text.Span($"33.761.757/0001-57").Underline();
                    text.Span("  ou à sua ordem, a quantia de");
                    text.Span($"  R$ {valorParcela} {HelperNumberToText.EscreverExtenso(valorParcela)}").Bold();
                    text.Span("  em moeda corrente, pagável em LONDRINA - PARANÁ");
                    text.AlignLeft();
                });
            });
        }

        private void ComposeParcialBody(IContainer container, CustomerModel customer, DateTime date)
        {
            container.Row(row =>
            {
                row.RelativeItem().PaddingTop(5).Text(text =>
                {
                    text.Line($"Emitente: {customer.Name}");
                    text.Line($"CPF/CNPJ: {customer.CpfOrCnpj}");
                    text.Line($"Endereço: {customer.Address}");
                });
                row.ConstantItem(50);
                row.RelativeItem().PaddingRight(5).PaddingBottom(0).Text(text =>
                {
                    text.Line($"Data de emissão: {DateTime.Now:dd/MM/yyyy}");
                    text.EmptyLine();
                    text.Line("                                                                                                  ").Underline();
                    text.Line($"{customer.Name}").Bold();
                    text.Line($"{customer.CpfOrCnpj}").Bold();
                    text.AlignRight();
                });
            });
        }
    }
}
