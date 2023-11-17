using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Pages
{
    public class GroupModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        private readonly ILogger<GroupModel> _logger;

        public GroupModel(ApplicationDbContext context, ILogger<GroupModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Group Group { get; set; }

        public async Task OnGetAsync(int id)
        {
            Group = await _context.Groups!.FirstOrDefaultAsync(g => g.GroupId == id);
            _context.Entry(Group).Collection(p => p.Tags!).Load();
            _context.Entry(Group).Collection(p => p.Lectors!).Load();
            _context.Entry(Group).Collection(p => p.Enrollments!).Load();
            _context.Entry(Group).Reference(p => p.Action!).Load();
        }
    }
}
