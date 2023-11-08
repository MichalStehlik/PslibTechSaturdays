using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PslibTechSaturdays.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        [Required]
        [DisplayName("Text")]
        public string Text { get; set; } = String.Empty;
        [Required]
        [DisplayName("Barva pozadí")]
        public string? BackgroundColor { get; set; } = string.Empty;
        [Required]
        [DisplayName("Barva popředí")]
        public string? ForegroundColor { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Group>? Groups { get; set; }
    }
}
