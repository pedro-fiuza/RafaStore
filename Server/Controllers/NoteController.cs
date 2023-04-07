using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RafaStore.Server.Services.FileService;

namespace RafaStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly IFileService _fileService;

        public NoteController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page)
        {
            return Ok(await _fileService.GetAllNotesPaginated(page));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomers(string startDate, string endDate, int page = 1)
        {
            return Ok(await _fileService.SearchNotes(startDate, endDate, page));
        }
    }
}
