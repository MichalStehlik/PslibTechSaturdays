using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Services;
using PslibTechSaturdays.ViewModels;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace PslibTechSaturdays.Pages
{
    public class ActionModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly EnrollmentsService _storage;

        public ActionModel(ILogger<IndexModel> logger, ApplicationDbContext context, EnrollmentsService storage)
        {
            _logger = logger;
            _context = context;
            _storage = storage;
        }

        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }

        public List<GroupEnrollmentVM> Groups { get; set; }
        public Models.Action Action { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            Action = await _context.Actions!.Where(x => x.ActionId == id).SingleOrDefaultAsync();
            if (Action is null)
            {
                return NotFound();
            }
            Groups = await _context.Groups.Where(x => x.ActionId == id)
                .Include(g => g.Enrollments)
                .Include(g => g.Tags)
                .Select(x => new GroupEnrollmentVM
                {
                    GroupId = x.GroupId,
                    Name = x.Name,
                    Tags = x.Tags!,
                    OpenedAt = x.OpenedAt,
                    ClosedAt = x.ClosedAt,
                    MinGrade = x.MinGrade,
                    Capacity = x.Capacity,
                    EnrollmentsCountVisible = x.EnrollmentsCountVisible,
                    EnrollmentsCount = x.Enrollments!.Count(),
                    ParticipantsCount = x.Enrollments!.Count(e => e.Cancelled == null),
                    UsersEnrollments = user != null ? x.Enrollments!.Count(e => e.ApplicationUserId == Guid.Parse(user.Value)) : 0,
                    ShortenedDescription = Regex.Replace(x.Description!, @"<[^>]+>|", "").Trim(),
                })
                .OrderBy(x => x.Name).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetEnrollAsync(int id)
        {
            var userId = User!.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            if (userId is null)
            {
                return BadRequest();
            }
            var result = await _storage.CreateAsync(Guid.Parse(userId), id, Guid.Parse(userId));
            switch (result) {
                case CreationResult.Success:
                    {
                        SuccessMessage = "P�ihl�ka byla vytvo�en�.";
                        return RedirectToPage("Enrollments", new { Area = "My" });
                    }
                case CreationResult.UnknownUser:
                    {
                        FailureMessage = "Nezn�m� u�ivatel.";
                        return RedirectToPage();
                    }
                case CreationResult.UnknownGroup:
                    {
                        FailureMessage = "Nezn�m� skupina.";
                        return RedirectToPage();
                    }
                case CreationResult.ExclusivityConflict:
                    {
                        FailureMessage = "U�ivatel nem��e b�t ve dvou skupin�ch z�rove�.";
                        return RedirectToPage();
                    }
                case CreationResult.ClosedGroup:
                    {
                        FailureMessage = "Skupina je uzav�en� pro z�pis.";
                        return RedirectToPage();
                    }
                case CreationResult.FullCapacity:
                    {
                        FailureMessage = "Skupina je ji� napln�n�.";
                        return RedirectToPage();
                    }
                case CreationResult.ConditionUnsatisfied:
                    {
                        FailureMessage = "Nebyla spln�na nutn� podm�nka.";
                        return RedirectToPage();
                    }
                case CreationResult.SQLError:
                    {
                        FailureMessage = "Chyba p�i z�pisu do datab�ze.";
                        return RedirectToPage();
                    }
                case CreationResult.FalseCurrentUser:
                    {
                        FailureMessage = "Chyba p�i identifikov�n� p�ihl�en�ho u�ivatele.";
                        return RedirectToPage();
                    }
                case CreationResult.EnrollmentDuplicity:
                    {
                        FailureMessage = "tato p�ihl�ka ji� existuje.";
                        return RedirectToPage();
                    }
                default:
                    {
                        FailureMessage = "Vytvo�en� p�ihl�ky se nepoda�ilo z nezn�m�ho d�vodu.";
                        return RedirectToPage();
                    }
            }
            
        }
    }
}
