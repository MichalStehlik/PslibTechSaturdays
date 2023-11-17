using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Helpers;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Services;
using PslibTechSaturdays.ViewModels;
using System.Security.Claims;

namespace PslibTechSaturdays.Areas.My.Pages
{
    public class EnrollmentsModel : PageModel
    {
        private readonly EnrollmentsService _storage;
        private readonly ILogger<EnrollmentsModel> _logger;
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public EnrollmentsModel(EnrollmentsService storage, ILogger<EnrollmentsModel> logger, ApplicationDbContext context)
        {
            _storage = storage;
            _logger = logger;
            _context = context;
        }

        public List<EnrollmentListVM> Enrollments { get; set; } = default!;
        public async Task<ActionResult> OnGetAsync()
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            if (userId is null)
            {
                return BadRequest();
            }
            if (_storage != null)
            {
                Enrollments = await _storage.ItemsAsync(Guid.Parse(userId),null,null,null,EnrollmentsOrder.CreatedDesc);
                return Page();
            }
            return NotFound();
        }

        public async Task<ActionResult> OnGetCancelAsync(int id)
        {
            var userId = User!.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            if (userId is null)
            {
                return BadRequest();
            }
            var enrollment = await _storage.GetAsync(id);
            if (enrollment is null)
            {
                return NotFound();
            }
            _context.Entry(enrollment)!.Reference(e => e.Group).Load();
            _context.Entry(enrollment.Group)!.Reference(g => g.Action).Load();
            if (enrollment.Group!.Action!.Start < DateTime.Now)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "P�ihl�ku ji� nelze zru�it.");
            }
            else
            {
                bool done = await _storage.CancelAsync(id, Guid.Parse(userId));
                if (done)
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "P�ihl�ka byla zru�en�.");
                }
                else
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "P�i ru�en� p�ihl�ky do�lo k chyb�.");
                }
            }    
            return RedirectToPage("Enrollments");
        }
    }
}
