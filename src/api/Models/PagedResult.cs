namespace CloudNotes.Api.Models
{
    public class PagedResult<T> where T : IEntity
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public List<T> Items { get; set; } = new List<T>();
    }
}
