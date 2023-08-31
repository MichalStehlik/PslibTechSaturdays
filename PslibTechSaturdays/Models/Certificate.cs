using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PslibTechSaturdays.Models
{
    public class Certificate
    {
        public Guid CertificateId { get; set; }
        public ApplicationUser User { get; set; } = new ApplicationUser();
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
        public Enrollment? Enrollment { get; set; }
    }
}
