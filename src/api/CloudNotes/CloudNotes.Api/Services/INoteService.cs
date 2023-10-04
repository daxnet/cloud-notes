using CloudNotes.Api.Models;

namespace CloudNotes.Api.Services
{
    public interface INoteService
    {
        Task<int> CreateNoteAsync(Note note, CancellationToken cancellationToken = default);

        Task<Note?> GetNoteByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<PagedResult<Note>> GetAllNotesAsync(
            string filterExpression = "",
            string sortExpression = "Id",
            SortOrder sortOrder = SortOrder.Ascending,
            int pageSize = 25, 
            int pageNumber = 0, 
            CancellationToken cancellationToken = default);

        Task<bool> DeleteNoteByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
