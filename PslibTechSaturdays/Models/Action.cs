using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace PslibTechSaturdays.Models
{
    public class Action
    {
        public int ActionId { get; set; }
        [Required]
        [DisplayName("Název")]
        public string Name { get; set; } = String.Empty;
        [DisplayName("Popis")]
        public string? Description { get; set; }
        [DisplayName("Školní rok")]
        public int Year { get; set; }
        [DisplayName("Aktivní")]
        public bool Active { get; set; } = true;
        [DisplayName("Zveřejněná")]
        public bool Published { get; set; } = false;
        [Column(TypeName = "datetime2")]
        [DisplayName("Založená")]
        public DateTime Created { get; set; } = DateTime.Now;
        [ForeignKey("CreatedById")]
        [DisplayName("Založil")]
        public ApplicationUser? CreatedBy { get; set; }
        public Guid CreatedById { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("Čas začátku")]
        public DateTime? Start { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("Čas konce")]
        public DateTime? End { get; set; }
        [JsonIgnore]
        public ICollection<Group>? Groups { get; set; }
    }
}
