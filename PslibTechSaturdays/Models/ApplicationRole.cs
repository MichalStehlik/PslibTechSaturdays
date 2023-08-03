using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PslibTechSaturdays.Models
{
    [Table("AspNetRoles")]
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ICollection<ApplicationUser>? Users { get; set; }
    }
}
