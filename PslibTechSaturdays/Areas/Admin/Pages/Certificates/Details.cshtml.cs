using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Services;

namespace PslibTechSaturdays.Areas.Admin.Pages.Certificates
{
    public class DetailsModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;
        private readonly CertificateGenerationService _cgs;

        public DetailsModel(ApplicationDbContext context, ILogger<DetailsModel> logger, CertificateGenerationService cgs)
        {
            _context = context;
            _logger = logger;
            _cgs = cgs;
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

        public async Task<ActionResult> OnGetDownloadHtmlAsync(Guid id)
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            Certificate? cert = await _context.Certificates.FirstOrDefaultAsync(x => x.CertificateId == id);
            if (cert == null)
            {
                return NotFound();
            }
            _context.Entry(cert).Reference(p => p.User).Load();
            string html = await _cgs.GetHtmlAsync(cert);
            return new ContentResult { Content = html, ContentType = "text/html" };
        }
    }
}
