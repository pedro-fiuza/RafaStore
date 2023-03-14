using RafaStore.Shared.Model;

namespace RafaStore.Shared.ViewModel
{
    public class CustomerListViewModel
    {
        public List<CustomerModel> Customers { get; set; } = new();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
