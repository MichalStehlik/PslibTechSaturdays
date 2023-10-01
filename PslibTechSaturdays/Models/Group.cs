using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace PslibTechSaturdays.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [Required]
        [DisplayName("Název")]
        public string Name { get; set; } = String.Empty;
        [DisplayName("Popis")]
        public string? Description { get; set; } = String.Empty;
        [Required]
        [DisplayName("Akce")]
        [ForeignKey("ActionId")]
        public Action? Action { get; set; }
        public int ActionId { get; set; }
        [DisplayName("Deklarovaná kapacita")]
        public int Capacity { get; set; }
        [DisplayName("Minimální třída")]
        public SchoolGrade MinGrade { get; set; } = SchoolGrade.None;
        [DisplayName("Veřejná poznámka")]
        public string? Note { get; set; }
        [DisplayName("Poznámka k lektorům")]
        public string? LectorsNote { get; set; }
        [JsonIgnore]
        public ICollection<ApplicationUser>? Lectors { get; set; }
        [DisplayName("Veřejný počet účastníků")]
        public bool EnrollmentsCountVisible { get; set; } = false;
        [Column(TypeName = "datetime2")]
        [DisplayName("Plánované otevření pro zápis")]
        public DateTime? PlannedOpening { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("Skutečné otevření pro zápis")]
        public DateTime? OpenedAt { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("Skutečné uzavření")]
        public DateTime? ClosedAt { get; set; }
        [DisplayName("Založil")]
        [ForeignKey("CreatedById")]
        public ApplicationUser? CreatedBy { get; set; }
        public Guid CreatedById { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("Založení")]
        public DateTime Created { get; set; } = DateTime.Now;
        [JsonIgnore]
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
