using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using System.ComponentModel.DataAnnotations;
using PslibTechSaturdays.Helpers;

namespace PslibTechSaturdays.Areas.Admin.Pages.Certificates
{
    public class EditModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;

        public EditModel(PslibTechSaturdays.Data.ApplicationDbContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public EditInputModel Input { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Certificates == null)
            {
                return NotFound();
            }

            var certificate =  await _context.Certificates.FirstOrDefaultAsync(m => m.CertificateId == id);
            if (certificate == null)
            {
                return NotFound();
            }
            Input = new EditInputModel
            {
                CertificateId = certificate.CertificateId,
                Title = certificate.Title,
                Text = certificate.Text,
                Description = certificate.Description,
            };
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

            var certificate = await _context.Certificates.FindAsync(Input.CertificateId);
            if (certificate == null)
            {
                return NotFound(ModelState);
            }
            certificate.Title = Input.Title;
            certificate.Text = Input.Text;
            certificate.Description = Input.Description;
            try
            {
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Certifikát byl aktualizován.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificateExists(Input.CertificateId))
                {
                    return NotFound();
                }
                else
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Aktualizace certifikátu se nepodařila.");
                }
            }
            return RedirectToPage("./Index");
        }

        private bool CertificateExists(Guid id)
        {
          return (_context.Certificates?.Any(e => e.CertificateId == id)).GetValueOrDefault();
        }
    }

    public class EditInputModel
    {
        public Guid CertificateId { get; set; }
        [Required]
        [DisplayName("Název")]
        public string Title { get; set; } = String.Empty;
        [DisplayName("Text")]
        public string Text { get; set; } = String.Empty;
        [DisplayName("Popis")]
        public string Description { get; set; } = String.Empty;
    }
}
