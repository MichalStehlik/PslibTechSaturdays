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
        public ApplicationUser User { get; set; } = new ApplicationUser();
        public Guid ApplicationUserId { get; set; }
        [Required]
        public Group Group { get; set; } = new Group();
        public int GroupId { get; set; }
        public ApplicationUser? CreatedBy { get; set; }
        [Required]
        [DisplayName("Založil")]
        public Guid CreatedById { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("Založeno")]
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
        [DisplayName("Zrušil")]
        public ApplicationUser? CancelledBy { get; set; }
        [DisplayName("Zrušeno")]
        public Guid CancelledById { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Cancelled { get; set; }
        [DisplayName("Prezence potvrzena")]
        public bool? Present { get; set; }

    }
}
