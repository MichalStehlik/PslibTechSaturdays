using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;

namespace PslibTechSaturdays.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public string? AppTitle { get; set; }
        public List<Models.Action> PublishedActions { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration, ApplicationDbContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            AppTitle = _configuration["Application:Name"];
            PublishedActions = await _context.Actions.Include(x => x.Groups).Where(x => x.Published == true).ToListAsync();
        }
    }
}