using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace PslibTechSaturdays.Models
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        [DisplayName("Jméno")]
        public string FirstName { get; set; } = String.Empty;
        [Required]
        [DisplayName("Příjmení")]
        public string LastName { get; set; } = String.Empty;
        [Column(TypeName = "datetime2")]
        [DisplayName("Datum narození")]
        public DateTime? BirthDate { get; set; }
        [DisplayName("Název školy")]
        public string? SchoolName { get; set; }
        [DisplayName("Třída")]
        public SchoolGrade Grade { get; set; } = SchoolGrade.None;
        [Column(TypeName = "datetime2")]
        [DisplayName("Vytvořeno")]
        public DateTime Created { get; set; } = DateTime.Now;
        [Column(TypeName = "datetime2")]
        [DisplayName("Aktualizováno")]
        public DateTime Updated { get; set; } = DateTime.Now;
        [DisplayName("Aktivní")]
        public bool Active { get; set; } = true;
        [DisplayName("Příhlášený k odběru zpráv")]
        public bool MailList { get; set; } = false;
        [DisplayName("Uchazeč o studium")]
        public bool Aspirant { get; set; } = false;
        [JsonIgnore]
        public ICollection<ApplicationRole>? Roles { get; set; }
        [JsonIgnore]
        public ICollection<Action>? CreatedActions { get; set; }
        [JsonIgnore]
        public ICollection<Enrollment>? Enrollments { get; set; }
        [JsonIgnore]
        public ICollection<Enrollment>? CreatedEnrollments { get; set; }
        [JsonIgnore]
        public ICollection<Group>? CreatedGroups { get; set; }
        [JsonIgnore]
        public ICollection<Enrollment>? CancelledEnrollments { get; set; }
        [JsonIgnore]
        public ICollection<Certificate>? Certificates { get; set; }
    }
}
