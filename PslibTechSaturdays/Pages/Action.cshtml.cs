using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Pages
{
    public class ActionModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public ActionModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Group> Groups { get; set; }
        public Models.Action Action { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            Action = await _context.Actions.Where(x => x.ActionId == id).SingleOrDefaultAsync();
            if (Action is null)
            {
                return NotFound();
            }
            Groups = await _context.Groups.Where(x => x.ActionId == id)
                .Include(g => g.Enrollments)
                .OrderBy(x => x.Name).ToListAsync();
            return Page();
        }
    }
}
