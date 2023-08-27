using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Emails.PageModels
{
    public class ConfirmEmailVM : EmailVM
    {
        public string ConfirmationCode { get; set; } = String.Empty;
        public ApplicationUser? User { get; set; }
        public string ConfirmEmailUrl { get; set; } = String.Empty;
        public string AppUrl { get; set; } = String.Empty;
    }
}
