using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RafaStore.Server.Services.FileService;

namespace RafaStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly IFileService _fileService;

        public NoteController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int customerId, int page)
        {
            return Ok(await _fileService.GetAllNotesPaginated(customerId, page));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomers(int customerId, string startDate, string endDate, int page = 1)
        {
            return Ok(await _fileService.SearchNotes(customerId, startDate, endDate, page));
        }

        [HttpDelete("delete-pdf/{noteId:int}")]
        public async Task<IActionResult> DeletePdf([FromRoute] int? noteId) 
        {
            return Ok(await _fileService.DeleteFile(noteId));
        }
    }
}
