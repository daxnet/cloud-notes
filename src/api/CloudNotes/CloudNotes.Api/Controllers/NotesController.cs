using CloudNotes.Api.Models;
using CloudNotes.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CloudNotes.Api.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly ILogger _logger;

        public NotesController(INoteService noteService, ILogger<NotesController> logger)
        {
            _noteService = noteService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetNoteByIdAsync(int id)
        {
            _logger.LogDebug($"Get note with Id={id}");
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note is null)
            {
                _logger.LogWarning($"Note Id={id} doesn't exist.");
                return BadRequest($"Note Id={id} doesn't exist.");
            }

            return Ok(note);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNotesAsync(
            [FromQuery] string filterExpression = "",
            [FromQuery] string sortField = "Id",
            [FromQuery] SortOrder sortOrder = SortOrder.Ascending,
            [FromQuery] int pageSize = 25, 
            [FromQuery] int pageNumber = 0)
        {
            var result = await _noteService.GetAllNotesAsync(filterExpression, sortField, sortOrder, pageSize, pageNumber).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateNoteAsync(Note note)
        {
            var id = await _noteService.CreateNoteAsync(note);
            return CreatedAtAction(nameof(GetNoteByIdAsync), new { id }, id);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleteResult = await _noteService.DeleteNoteByIdAsync(id).ConfigureAwait(false);
            return deleteResult ? NoContent() : BadRequest($"Note Id={id} doesn't exist.");
        }
    }
}
