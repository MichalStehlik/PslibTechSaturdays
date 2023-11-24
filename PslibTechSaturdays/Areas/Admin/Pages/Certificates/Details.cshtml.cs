using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Certificates
{
    public class DetailsModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public DetailsModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Certificate Certificate { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Certificates == null)
            {
                return NotFound();
            }

            var certificate = await _context.Certificates.FirstOrDefaultAsync(m => m.CertificateId == id);
            if (certificate == null)
            {
                return NotFound();
            }
            else 
            {
                _context.Entry(certificate).Reference(c => c.User).Load();
                _context.Entry(certificate).Reference(c => c.CreatedBy).Load();
                _context.Entry(certificate).Reference(c => c.Enrollment).Load();
                if (certificate.Enrollment != null) _context.Entry(certificate.Enrollment).Reference(c => c.Group).Load();
                Certificate = certificate;
            }
            return Page();
        }
    }
}
