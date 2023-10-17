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
                        SuccessMessage = "Pøihláška byla vytvoøená.";
                        return RedirectToPage("Enrollments", new { Area = "My" });
                    }
                case CreationResult.UnknownUser:
                    {
                        FailureMessage = "Neznámý uživatel.";
                        return RedirectToPage();
                    }
                case CreationResult.UnknownGroup:
                    {
                        FailureMessage = "Neznámá skupina.";
                        return RedirectToPage();
                    }
                case CreationResult.ExclusivityConflict:
                    {
                        FailureMessage = "Uživatel nemùže být ve dvou skupinách zároveò.";
                        return RedirectToPage();
                    }
                case CreationResult.ClosedGroup:
                    {
                        FailureMessage = "Skupina je uzavøená pro zápis.";
                        return RedirectToPage();
                    }
                case CreationResult.FullCapacity:
                    {
                        FailureMessage = "Skupina je již naplnìná.";
                        return RedirectToPage();
                    }
                case CreationResult.ConditionUnsatisfied:
                    {
                        FailureMessage = "Nebyla splnìna nutná podmínka.";
                        return RedirectToPage();
                    }
                case CreationResult.SQLError:
                    {
                        FailureMessage = "Chyba pøi zápisu do databáze.";
                        return RedirectToPage();
                    }
                case CreationResult.FalseCurrentUser:
                    {
                        FailureMessage = "Chyba pøi identifikování pøihlášeného uživatele.";
                        return RedirectToPage();
                    }
                case CreationResult.EnrollmentDuplicity:
                    {
                        FailureMessage = "tato pøihláška již existuje.";
                        return RedirectToPage();
                    }
                default:
                    {
                        FailureMessage = "Vytvoøení pøihlášky se nepodaøilo z neznámého dùvodu.";
                        return RedirectToPage();
                    }
            }
            
        }
    }
}
