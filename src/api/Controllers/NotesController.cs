using CloudNotes.Api.Models;
using CloudNotes.Api.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CloudNotes.Api.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService, ILogger<NotesController> logger)
        {
            _noteService = noteService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new note in the system.
        /// </summary>
        /// <param name="note">The note to be created.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateNoteAsync(Note note)
        {
            var id = await _noteService.CreateNoteAsync(note);
            // ReSharper disable once Mvc.ActionNotResolved
            return CreatedAtAction(nameof(GetNoteByIdAsync), new { id }, id);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleteResult = await _noteService.DeleteNoteByIdAsync(id);
            return deleteResult ? NoContent() : BadRequest($"Note Id={id} doesn't exist.");
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
            [FromQuery] int pageNumber = 0) =>
            Ok(await _noteService.GetAllNotesAsync(filterExpression, sortField, sortOrder, pageSize, pageNumber));

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchByIdAsync(int id, [FromBody] JsonPatchDocument<Note>? patchDoc)
        {
            if (patchDoc != null)
            {
                var note = await _noteService.GetNoteByIdAsync(id);
                if (note != null)
                {
                    patchDoc.ApplyTo(note, ModelState);
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    await _noteService.UpdateNoteAsync(id, note);
                    return NoContent();
                }

                return BadRequest($"Note Id={id} doesn't exist.");
            }

            return BadRequest(ModelState);
        }
    }
}