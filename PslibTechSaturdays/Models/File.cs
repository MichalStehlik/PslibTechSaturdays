using Microsoft.AspNetCore.Identity;

namespace PslibTechSaturdays.Models
{
    public class File
    {
        public Guid FileId { get; set; }
        public string OriginalName { get; set; } = String.Empty;
        public ApplicationUser Uploader { get; set; }
        public Guid UploaderId { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
