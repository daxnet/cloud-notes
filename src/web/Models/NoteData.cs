using System.Globalization;
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

        [JsonIgnore]
        public string DateCreatedRepresentation
        {
            get
            {
                if (DateTime.UtcNow - DateCreatedUtc < TimeSpan.FromMinutes(1))
                {
                    return "刚刚";
                }
                else if (DateTime.UtcNow - DateCreatedUtc < TimeSpan.FromDays(1))
                {
                    return $"今天 {DateCreatedUtc.ToLocalTime().ToShortTimeString()}";
                }
                else if (DateTime.UtcNow - DateCreatedUtc < TimeSpan.FromDays(2))
                {
                    return $"昨天 {DateCreatedUtc.ToLocalTime().ToShortTimeString()}";
                }

                return DateCreatedUtc.ToLocalTime().ToString(CultureInfo.CurrentCulture);
            }
        }
    }
}
