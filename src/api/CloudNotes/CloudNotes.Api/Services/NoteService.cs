using CloudNotes.Api.Models;
using Dapper;
using Npgsql;
using System.Text;

namespace CloudNotes.Api.Services
{
    public class NoteService : INoteService
    {
        private readonly string _connectionString;

        public NoteService(IConfiguration configuration)
        {
            _connectionString = configuration["db:connectionString"] ?? throw new InvalidOperationException("The connectionString is not specified.");
        }

        public async Task<int> CreateNoteAsync(Note note, CancellationToken cancellationToken = default)
        {
            if (note == null)
                throw new ArgumentNullException(nameof(note));
            if (note.DateCreated == default)
            {
                note.DateCreated = DateTime.UtcNow;
            }

            const string sql = "INSERT INTO notes (\"Title\", \"Content\", \"DateCreated\") VALUES (@Title, @Content, @DateCreated) RETURNING \"Id\"";
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>(sql, note).ConfigureAwait(false);
        }

        public async Task<bool> DeleteNoteByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM notes WHERE \"Id\"=@id";
            await using var connection = new NpgsqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(sql, new { id }).ConfigureAwait(false);
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateNoteAsync(int id, Note note, CancellationToken cancellationToken = default)
        {
            const string sql = "UPDATE notes SET \"Title\"=@Title, \"Content\"=@Content WHERE \"Id\"=@id";
            await using var connection = new NpgsqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(sql, new { note.Title, note.Content, id })
                .ConfigureAwait(false);
            return rowsAffected > 0;
        }

        public async Task<PagedResult<Note>> GetAllNotesAsync(
            string filterExpression = "",
            string sortExpression = "Id",
            SortOrder sortOrder = SortOrder.Ascending,
            int pageSize = 25,
            int pageNumber = 0, 
            CancellationToken cancellationToken = default)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT *, COUNT(*) OVER() AS \"TotalCount\" FROM notes ");
            if (!string.IsNullOrEmpty(filterExpression))
            {
                sqlBuilder.Append($"WHERE {filterExpression} ");
            }
            if (!string.IsNullOrEmpty(sortExpression))
            {
                sqlBuilder.Append($"ORDER BY \"{sortExpression}\" ");
                if (sortOrder == SortOrder.Descending)
                {
                    sqlBuilder.Append("DESC ");
                }
            }

            var offset = pageNumber * pageSize;
            sqlBuilder.Append($"OFFSET {offset} LIMIT {pageSize}");

            await using var connection = new NpgsqlConnection(_connectionString);
            var allNotes = await connection.QueryAsync<NoteWithTotalCount>(sqlBuilder.ToString());
            var noteWithTotalCounts = allNotes.ToList();
            var totalCount = noteWithTotalCounts.FirstOrDefault()?.TotalCount ?? 0;
            var totalPages = Convert.ToInt32(Math.Ceiling(totalCount * 1.0F / pageSize));

            var result = new PagedResult<Note>()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            result.Items.AddRange(noteWithTotalCounts);

            return result;
        }

        public async Task<Note?> GetNoteByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM notes WHERE \"Id\"=@Id";
            await using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Note?>(sql, new { Id = id });
        }
    }
}
