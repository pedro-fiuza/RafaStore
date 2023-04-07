using RafaStore.Shared;
using RafaStore.Shared.Model;
using RafaStore.Shared.ViewModel;

namespace RafaStore.Server.Services.HospitalService
{
    public interface ICustomerService
    {
        Task<ServiceResponse<CustomerModel>> Add(CustomerModel customer);
        Task<ServiceResponse<CustomerModel>> Update(CustomerModel customer);
        Task<ServiceResponse<CustomerListViewModel>> SearchCustomers(string searchText, int page);
        Task<ServiceResponse<CustomerModel>> GetCustomerById(int id);
        Task<ServiceResponse<CustomerListViewModel>> GetAllCustomersPaginated(int page);
        Task<byte[]> GeneratePdf(GeneratePdfViewModel customer);
        Task<byte[]> DownloadCustomerNote(int noteId);
    }
}
