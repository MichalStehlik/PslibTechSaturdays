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

namespace PslibTechSaturdays.Areas.My.Pages
{
    public class CertificatesModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }

        public CertificatesModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Certificate> Certificates { get;set; } = default!;

        public async Task<ActionResult> OnGetAsync()
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
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
    }
}
