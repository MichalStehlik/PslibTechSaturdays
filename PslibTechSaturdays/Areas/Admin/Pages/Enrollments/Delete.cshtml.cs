using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Enrollments
{
    public class DeleteModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public DeleteModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Enrollment Enrollment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(m => m.EnrollmentId == id);

            if (enrollment == null)
            {
                return NotFound();
            }
            else 
            {
                Enrollment = enrollment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment != null)
            {
                Enrollment = enrollment;
                _context.Enrollments.Remove(Enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
