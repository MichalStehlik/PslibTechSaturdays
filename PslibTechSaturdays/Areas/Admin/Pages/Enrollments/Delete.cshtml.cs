using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Services;

namespace PslibTechSaturdays.Areas.Admin.Pages.Enrollments
{
    public class DeleteModel : PageModel
    {
        private readonly EnrollmentsService _storage;
        private readonly ILogger<IndexModel> _logger;

        public DeleteModel(EnrollmentsService storage, ILogger<IndexModel> logger)
        {
            _storage = storage;
            _logger = logger;
        }

        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }

        [BindProperty]
        public Enrollment Enrollment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _storage == null)
            {
                return NotFound();
            }

            var enrollment = await _storage.GetAsync((int)id);

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
            if (id == null || _storage == null)
            {
                return NotFound();
            }
            var enrollment = await _storage.GetAsync((int)id);

            if (enrollment != null)
            {
                try
                {
                    await _storage.RemoveAsync((int)id);
                    SuccessMessage = "Přihláška byla odstraněna.";
                }
                catch
                {
                    FailureMessage = "Přihlášku se nepodařilo odstranit.";
                }
                
            }

            return RedirectToPage("./Index");
        }
    }
}
