using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Helpers;
using System.Net;
using static PslibTechSaturdays.Helpers.TempDataExtension;

namespace PslibTechSaturdays.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public string? AppTitle { get; set; }
        public List<Models.Action> PublishedActions { get; set; }
        public string IntroductionHTMLText { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration, ApplicationDbContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            AppTitle = _configuration["Application:Name"];
            IntroductionHTMLText = _configuration["TitlePage:Introduction"] ?? "<p>?</p>";

            PublishedActions = await _context.Actions.Include(x => x.Groups).Where(x => x.Published == true).ToListAsync();
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(365),
                HttpOnly = false,
                Secure = false,
            };
            Response.Cookies.Append("last-access", DateTime.Now.ToString(), options);
        }

        public IActionResult OnGetMessage(string text, MessageType type = MessageType.Info)
        {
            //infobox = text;

            TempData.AddMessage("infobox", type, text);
            return RedirectToPage();
        }
    }
}