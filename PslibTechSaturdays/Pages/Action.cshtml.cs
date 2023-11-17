using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Helpers;
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

        public List<GroupEnrollmentVM> Groups { get; set; }
        public Models.Action Action { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            Action = await _context.Actions!.Where(x => x.ActionId == id).SingleOrDefaultAsync();
            if (Action is null)
            {
                return NotFound();
            }
            Groups = await _context.Groups.Where(x => x.ActionId == Action.ActionId)
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
                    ShortenedDescription = !String.IsNullOrEmpty(x.Description) ? Regex.Replace(x.Description, @"<[^>]+>|", "").Trim() : null,
                })
                .OrderBy(x => x.Name).ToListAsync();
            if (user != null)
            {
                Enrollments = await _context.Enrollments
                    .Include(x => x.Group)
                    .Where(x => 
                    x.ApplicationUserId == Guid.Parse(user!.Value)
                    && x.Cancelled == null
                    && x.Group.ActionId == id
                    ).ToListAsync() ?? new List<Enrollment>();
            }
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
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Pøihláška byla vytvoøena.");
                        return RedirectToPage("Enrollments", new { Area = "My" });
                    }
                case CreationResult.UnknownUser:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Neznámý uživatel.");
                        return RedirectToPage();
                    }
                case CreationResult.UnknownGroup:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Neznámá skupina.");
                        return RedirectToPage();
                    }
                case CreationResult.ExclusivityConflict:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Uživatel nemùže být ve více skupinách.");
                        return RedirectToPage();
                    }
                case CreationResult.ClosedGroup:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Skupina je uzavøená pro zápis.");
                        return RedirectToPage();
                    }
                case CreationResult.FullCapacity:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Skupina je již naplnìná.");
                        return RedirectToPage();
                    }
                case CreationResult.ConditionUnsatisfied:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nebyla splnìna potøebná podmínka.");
                        return RedirectToPage();
                    }
                case CreationResult.SQLError:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Chyba pøi zápisu do databáze.");
                        return RedirectToPage();
                    }
                case CreationResult.FalseCurrentUser:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Pøihlášený uživatel nemá platnou identifikaci.");
                        return RedirectToPage();
                    }
                case CreationResult.EnrollmentDuplicity:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Pøihláška již existuje.");
                        return RedirectToPage();
                    }
                default:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Pøihlášku se nepodaøilo vytvoøit z neznámého dùvodu.");
                        return RedirectToPage();
                    }
            }
            
        }
    }
}
