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

namespace PslibTechSaturdays.Areas.Admin.Pages.Certificates
{
    public class DeleteModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public DeleteModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
                Certificate = certificate;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Certificates == null)
            {
                return NotFound();
            }
            var certificate = await _context.Certificates.FindAsync(id);

            if (certificate != null)
            {
                try
                {
                    Certificate = certificate;
                    _context.Entry(Certificate).Reference(p => p.Enrollment).Load();
                    if (Certificate.Enrollment != null)
                    {
                        var enroll = Certificate.Enrollment;
                        enroll.CertificateId = null;
                        await _context.SaveChangesAsync();
                    }
                    _context.Certificates.Remove(Certificate);
                    await _context.SaveChangesAsync();
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Certifikát byl odstraněn.");
                }
                catch
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Certifikát se nepodařilo odstranit.");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
