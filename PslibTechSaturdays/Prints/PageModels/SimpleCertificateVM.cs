using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Prints.PageModels
{
    public class SimpleCertificateVM
    {
        public string Title { get; set; } = String.Empty;
        public string Text { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public ApplicationUser? User { get; set; }
        public Guid CertificateId { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
