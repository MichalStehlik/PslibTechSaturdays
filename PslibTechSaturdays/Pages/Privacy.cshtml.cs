using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PslibTechSaturdays.Helpers;
using static PslibTechSaturdays.Helpers.TempDataExtension;

namespace PslibTechSaturdays.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public void OnGetMessage(string text, MessageType type = MessageType.Info)
        {
            //infobox = text;

            TempData.AddMessage("infobox", type, text);
        }
    }
}