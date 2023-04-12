using Microsoft.JSInterop;
using RafaStore.Shared.Model;
using RafaStore.Shared.ViewModel;
using System.Net;

namespace RafaStore.Client.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient httpClient;
        public List<CustomerModel> Customers { get; set; } = new();
        public string Message { get; set; } = "Carregando clientes...";
        public event Action CustomersChanged;
        public string LastSearchText { get; set; } = string.Empty;
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public bool ReOrderDescending { get; set; } = true;

        public CustomerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CustomerModel> CreateCustomer(CustomerModel customer)
        {
            var result = await httpClient.PostAsJsonAsync("api/customer", customer);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<CustomerModel>>()).Data;
        }

        public async Task<byte[]> GeneratePdf(GeneratePdfViewModel note)
        {
            var file = await httpClient.PostAsJsonAsync("api/customer/generate-pdf", note);
            return await file.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> DownloadPdf()
        {
            var file = await httpClient.PostAsJsonAsync("api/customer/teste", new object());
            return await file.Content.ReadAsByteArrayAsync();
        }

        public async Task<bool> DeleteCustomer(int? customerId)
        {
            var file = await httpClient.DeleteAsync($"api/customer/delete-customer?customerId={customerId}");
            return file.StatusCode.Equals(HttpStatusCode.OK);
        }

        public async Task<ServiceResponse<CustomerModel>> GetCustomerById(int id)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<CustomerModel>>($"api/customer/{id}");
            return result;
        }

        public async Task SearchCustomers(string searchText, int page)
        {
            LastSearchText = string.IsNullOrEmpty(searchText) ? LastSearchText : searchText;
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<CustomerListViewModel>>($"api/customer/search?searchText={LastSearchText}&page={page}");

            if (result != null && result.Data != null)
            {
                Customers = result.Data.Customers;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            if (Customers.Count == 0) Message = "Nenhum cliente foi encontrado...";
            CustomersChanged?.Invoke();
        }

        public async Task<CustomerModel> UpdateCustomer(CustomerModel customer)
        {
            var result = await httpClient.PutAsJsonAsync("api/customer", customer);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<CustomerModel>>()).Data;
        }

        public async Task GetAllCustomersPaginated(int page)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<CustomerListViewModel>>($"api/customer?page={page}");

            if (result != null && result.Data != null)
            {
                Customers = result.Data.Customers;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            if (Customers.Count == 0) Message = "Nenhum cliente foi encontrado...";

            CustomersChanged?.Invoke();
        }

        public async Task ReOrder()
        {
            if (ReOrderDescending)
            {
                Customers = Customers.OrderByDescending(x => x.Name).ToList();
                ReOrderDescending = false;
            }
            else
            {
                Customers = Customers.OrderBy(x => x.Name).ToList();
                ReOrderDescending = true;
            }

            CustomersChanged?.Invoke();
        }
    }
}
