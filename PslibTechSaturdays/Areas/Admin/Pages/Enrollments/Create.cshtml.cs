using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Enrollments
{
    public class CreateModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public CreateModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CancelledById"] = new SelectList(_context.Users, "Id", "FirstName");
        ViewData["CertificateId"] = new SelectList(_context.Certificates, "CertificateId", "CertificateId");
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FirstName");
        ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "Name");
        ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return Page();
        }

        [BindProperty]
        public Enrollment Enrollment { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Enrollments == null || Enrollment == null)
            {
                return Page();
            }

            _context.Enrollments.Add(Enrollment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
