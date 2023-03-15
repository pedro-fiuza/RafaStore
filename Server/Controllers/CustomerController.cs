using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RafaStore.Shared.Model;
using RafaStore.Shared.ViewModel;

namespace RafaStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page)
        {
            return Ok(await _customerService.GetAllCustomersPaginated(page));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerModel customer)
        {
            var response = await _customerService.Add(customer);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(CustomerModel customer)
        {
            var response = await _customerService.Update(customer);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var response = await _customerService.GetCustomerById(id);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomers(string searchText, int page = 1)
        {
            return Ok(await _customerService.SearchCustomers(searchText, page));
        }

        [HttpPost("generate-pdf")]
        public IActionResult GeneratePdf(GeneratePdfViewModel note)
        {
            try
            {
                return File(_customerService.GeneratePdf(note), "application/pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }
}
