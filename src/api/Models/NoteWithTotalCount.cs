using Newtonsoft.Json;

namespace CloudNotes.Api.Models
{
    public class NoteWithTotalCount : Note
    {
        [JsonIgnore]
        public int TotalCount { get; set; }
    }
}
