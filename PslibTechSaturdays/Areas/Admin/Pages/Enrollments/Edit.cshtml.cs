using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Enrollments
{
    public class EditModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public EditModel(PslibTechSaturdays.Data.ApplicationDbContext context)
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

            var enrollment =  await _context.Enrollments.FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }
            Enrollment = enrollment;
           ViewData["CancelledById"] = new SelectList(_context.Users, "Id", "FirstName");
           ViewData["CertificateId"] = new SelectList(_context.Certificates, "CertificateId", "CertificateId");
           ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FirstName");
           ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "Name");
           ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Enrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(Enrollment.EnrollmentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EnrollmentExists(int id)
        {
          return (_context.Enrollments?.Any(e => e.EnrollmentId == id)).GetValueOrDefault();
        }
    }
}
