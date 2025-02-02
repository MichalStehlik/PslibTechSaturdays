using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Helpers;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Groups
{
    public class DeleteModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public DeleteModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Group Group { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.FirstOrDefaultAsync(m => m.GroupId == id);

            if (group == null)
            {
                return NotFound();
            }
            else 
            {
                _context.Entry(group).Reference(p => p.CreatedBy).Load();
                _context.Entry(group).Reference(p => p.Action).Load();
                Group = group;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }
            var group = await _context.Groups.FindAsync(id);

            if (group != null)
            {
                try
                {
                    var enrollments = await _context.Enrollments.Where(e => e.GroupId == group.GroupId).ToListAsync();
                    foreach (var enrollment in enrollments)
                    {
                        _context.Enrollments.Remove(enrollment);
                    }
                    await _context.SaveChangesAsync();
                    Group = group;
                    _context.Groups.Remove(Group);
                    await _context.SaveChangesAsync();
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Skupina byla odstraněna.");
                }
                catch(Exception ex)
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Při odstraňování skupiny došlo k chybě: " + ex.Message);
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
