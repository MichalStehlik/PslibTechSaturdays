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

namespace PslibTechSaturdays.Areas.My.Pages
{
    public class CertificatesModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        private readonly ILogger<CertificatesModel> _logger;
        private readonly CertificateGenerationService _cgs;

        public CertificatesModel(ApplicationDbContext context, ILogger<CertificatesModel> logger, CertificateGenerationService cgs)
        {
            _context = context;
            _logger = logger;
            _cgs = cgs;
        }

        public IList<Certificate> Certificates { get;set; } = default!;

        public async Task<ActionResult> OnGetAsync()
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            if (userId is null)
            {
                return BadRequest();
            }
            if (_context.Certificates != null)
            {
                Certificates = await _context.Certificates
                .Include(c => c.CreatedBy)
                .Where(x => x.UserId == Guid.Parse(userId))
                .OrderByDescending(x => x.Issued)
                .ToListAsync();
                return Page();
            }
            return NotFound();
        }

        public async Task<ActionResult> OnGetDownloadHtmlAsync(Guid id)
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            Certificate? cert = await _context.Certificates.FirstOrDefaultAsync(x => x.CertificateId == id);
            if (cert == null)
            {
                return NotFound();
            }
            if (cert.UserId != Guid.Parse(userId))
            {
                return Unauthorized();
            }
            _context.Entry(cert).Reference(p => p.User).Load();
            string html = await _cgs.GetHtmlAsync(cert);
            return new ContentResult { Content = html, ContentType="text/html"};
        }
    }
}
