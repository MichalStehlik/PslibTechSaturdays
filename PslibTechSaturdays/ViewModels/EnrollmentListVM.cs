using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PslibTechSaturdays.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PslibTechSaturdays.ViewModels
{
    public class EnrollmentListVM
    {
        public int EnrollmentId { get; set; }
        [Required]
        [DisplayName("Uživatel")]
        public ApplicationUser User { get; set; } = new ApplicationUser();
        [Required]
        [DisplayName("Skupina")]
        public Group Group { get; set; } = new Group();
        [DisplayName("Akce")]
        public Models.Action? Action { get; set; }
        [DisplayName("Založil")]
        [Column(TypeName = "datetime2")]
        public ApplicationUser? CreatedBy { get; set; }
        [Required]
        [DisplayName("Založeno")]
        public DateTime Created { get; set; } = DateTime.Now;
        [DisplayName("Zrušil")]
        public ApplicationUser? CancelledBy { get; set; }
        [DisplayName("Zrušeno")]
        [Column(TypeName = "datetime2")]
        public DateTime? Cancelled { get; set; }
        [DisplayName("Přítomen")]
        public Presence Present { get; set; }
        [DisplayName("Osvědčení")]
        public Certificate? Certificate { get; set; }
    }
}
