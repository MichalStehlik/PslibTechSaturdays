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

namespace PslibTechSaturdays.Areas.Admin.Pages.Enrollments
{
    public class DetailsModel : PageModel
    {
        private readonly EnrollmentsService _storage;
        private readonly ILogger<IndexModel> _logger;

        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }

        public DetailsModel(EnrollmentsService storage, ILogger<IndexModel> logger)
        {
            _storage = storage;
            _logger = logger;
        }

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

        public async Task<IActionResult> OnGetCancelAsync(int? id)
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
                var userId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                await _storage.CancelAsync((int)id, userId);
                Enrollment = enrollment;
            }
            return Page();
        }

        public async Task<IActionResult> OnGetPresenceAsync(int? id, Presence state)
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
                await _storage.SetPresenceAsync((int)id, state);
                Enrollment = enrollment;
            }
            return Page();
        }
    }
}
