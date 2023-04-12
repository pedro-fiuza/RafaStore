using RafaStore.Shared.Model;
using RafaStore.Shared.ViewModel;

namespace RafaStore.Client.Services.CustomerService
{
    public interface ICustomerService
    {
        string LastSearchText { get; set; }
        string Message { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        bool ReOrderDescending { get; set; }
        event Action CustomersChanged;
        List<CustomerModel> Customers { get; set; }
        Task SearchCustomers(string searchText, int page);
        Task ReOrder();
        Task<CustomerModel> CreateCustomer(CustomerModel customer);
        Task<bool> DeleteCustomer(int? customerId);
        Task GetAllCustomersPaginated(int page);
        Task<ServiceResponse<CustomerModel>> GetCustomerById(int id);
        Task<CustomerModel> UpdateCustomer(CustomerModel Customer);
        Task<byte[]> GeneratePdf(GeneratePdfViewModel note);
        Task<byte[]> DownloadPdf();
    }
}
