using System.Text.Json.Serialization;

namespace PslibTechSaturdays.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Text { get; set; } = String.Empty;
        public string? BackgroundColor { get; set; } = string.Empty;
        public string? ForegroundColor { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Group>? Groups { get; set; }
    }
}
