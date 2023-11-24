using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PslibTechSaturdays.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        [Required]
        [DisplayName("Uživatel")]
        public ApplicationUser User { get; set; } = new ApplicationUser();
        public Guid ApplicationUserId { get; set; }
        [Required]
        [DisplayName("Skupina")]
        public Group Group { get; set; } = new Group();
        public int GroupId { get; set; }
        [DisplayName("Založil")]
        public ApplicationUser? CreatedBy { get; set; }
        [Required]   
        public Guid CreatedById { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("Založeno")]
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
        [DisplayName("Zrušil")]
        public ApplicationUser? CancelledBy { get; set; }
        public Guid? CancelledById { get; set; }
        [DisplayName("Zrušeno")]
        [Column(TypeName = "datetime2")]
        public DateTime? Cancelled { get; set; }
        [DisplayName("Prezence")]
        public Presence Present { get; set; } = Presence.Unknown;
        public Guid? CertificateId { get; set; } = null;
        [DisplayName("Osvědčení")]
        public Certificate? Certificate { get; set; }
    }
}
