using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using System.Security.Claims;

namespace PslibTechSaturdays.Areas.Lectoring.Pages.Groups
{
    public class IndexModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Group> Groups { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            Groups = await _context.Groups.Include(x => x.Lectors).Include(x => x.Action)
                .Where(x => x.Lectors!.Any(x => x.Id == Guid.Parse(userId)))
                .OrderByDescending(x => x.CreatedBy)
                .ToListAsync();
            return Page();
        }
    }
}
