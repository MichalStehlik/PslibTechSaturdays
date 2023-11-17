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
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Přihláška byla odstraněna.");
                }
                catch
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Přihlášku se nepodařilo odstranit.");
                }
                
            }

            return RedirectToPage("./Index");
        }
    }
}
