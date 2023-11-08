using PslibTechSaturdays.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace PslibTechSaturdays.ViewModels
{
    public class CertificateListVM
    {
        public Guid CertificateId { get; set; }
        [DisplayName("Uživatel")]
        public ApplicationUser? User { get; set; }
        public Guid UserId { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("Datum vydání")]
        public DateTime Issued { get; set; }
        [DisplayName("Založil")]
        public ApplicationUser? CreatedBy { get; set; }
        public Guid CreatedById { get; set; }
        [DisplayName("Titulek")]
        public string Title { get; set; } = String.Empty;
        [DisplayName("Popisek")]
        public string Description { get; set; } = String.Empty;
        [DisplayName("Text")]
        public string Text { get; set; } = String.Empty;
        public Enrollment? Enrollment { get; set; }
        public int? EnrollmentId { get; set; }
    }
}
