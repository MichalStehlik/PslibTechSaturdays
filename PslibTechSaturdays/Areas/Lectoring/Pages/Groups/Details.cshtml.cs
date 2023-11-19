using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Lectoring.Pages.Groups
{
    public class DetailsModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(ApplicationDbContext context, ILogger<DetailsModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Group Group { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }
            Group = await _context!.Groups!.FirstOrDefaultAsync(m => m.GroupId == id);
            _context.Entry(Group).Reference(p => p.Action).Load();
            return Page();
        }
    }
}
