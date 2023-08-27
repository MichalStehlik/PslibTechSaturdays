using System.ComponentModel.DataAnnotations.Schema;

namespace PslibTechSaturdays.Models
{
    public class LectorAssignment
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
