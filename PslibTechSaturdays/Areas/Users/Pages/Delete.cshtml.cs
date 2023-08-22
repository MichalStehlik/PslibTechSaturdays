using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Users.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public DeleteModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public ApplicationUser ApplicationUser { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var applicationuser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (applicationuser == null)
            {
                return NotFound();
            }
            else 
            {
                ApplicationUser = applicationuser;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var applicationuser = await _context.Users.FindAsync(id);

            if (applicationuser != null)
            {
                ApplicationUser = applicationuser;
                _context.Users.Remove(ApplicationUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
