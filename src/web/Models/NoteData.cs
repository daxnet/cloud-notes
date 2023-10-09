namespace CloudNotes.Web.Models
{
    public class NoteData
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
    }
}
