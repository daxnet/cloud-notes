using System.Text.Json.Serialization;

namespace CloudNotes.Web.Models
{
    public class NoteData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        
        [JsonPropertyName("content")]
        public string? Content { get; set; }
        
        [JsonPropertyName("dateCreated")]
        public DateTime DateCreatedUtc { get; set; }
        
        [JsonPropertyName("dateModified")]
        public DateTime? DateModifiedUtc { get; set; }
    }
}
