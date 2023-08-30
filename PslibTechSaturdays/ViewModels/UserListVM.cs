using PslibTechSaturdays.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PslibTechSaturdays.ViewModels
{
    public class UserListVM
    {
        public Guid Id { get; set; }
        [Required]
        [DisplayName("Jméno")]
        public string FirstName { get; set; } = String.Empty;
        [Required]
        [DisplayName("Příjmení")]
        public string LastName { get; set; } = String.Empty;
        [Required]
        [DisplayName("Email")]
        public string? Email { get; set; } = String.Empty;
        [Column(TypeName = "datetime2")]
        [DisplayName("Narození")]
        public DateTime? BirthDate { get; set; }
        [DisplayName("Škola")]
        public string? SchoolName { get; set; }
        [DisplayName("Třída")]
        public SchoolGrade Grade { get; set; } = SchoolGrade.None;
        [Column(TypeName = "datetime2")]
        [DisplayName("Vytvořeno")]
        public DateTime Created { get; set; } = DateTime.Now;
        [DisplayName("Aktivní")]
        public bool Active { get; set; } = true;
        [DisplayName("Odběr")]
        public bool MailList { get; set; } = false;
        [DisplayName("Uchazeč")]
        public bool Aspirant { get; set; } = false;
        [DisplayName("Role")]
        [JsonIgnore]
        public ICollection<ApplicationRole>? Roles { get; set; }
        public bool Admin { get; set; }
        public bool Lector { get; set; }
        [JsonIgnore]
        public ICollection<Enrollment>? Enrollments { get; set; }
        [DisplayName("Přihlášky")]
        public int EnrollmentsCount { get; set; } = 0;
        [DisplayName("Certifikáty")]
        public int CertificatesCount { get; set; } = 0;
    }
}
