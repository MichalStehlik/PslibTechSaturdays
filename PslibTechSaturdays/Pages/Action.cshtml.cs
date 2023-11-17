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
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "P�ihl�ka byla vytvo�ena.");
                        return RedirectToPage("Enrollments", new { Area = "My" });
                    }
                case CreationResult.UnknownUser:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nezn�m� u�ivatel.");
                        return RedirectToPage();
                    }
                case CreationResult.UnknownGroup:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nezn�m� skupina.");
                        return RedirectToPage();
                    }
                case CreationResult.ExclusivityConflict:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "U�ivatel nem��e b�t ve v�ce skupin�ch.");
                        return RedirectToPage();
                    }
                case CreationResult.ClosedGroup:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Skupina je uzav�en� pro z�pis.");
                        return RedirectToPage();
                    }
                case CreationResult.FullCapacity:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Skupina je ji� napln�n�.");
                        return RedirectToPage();
                    }
                case CreationResult.ConditionUnsatisfied:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nebyla spln�na pot�ebn� podm�nka.");
                        return RedirectToPage();
                    }
                case CreationResult.SQLError:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Chyba p�i z�pisu do datab�ze.");
                        return RedirectToPage();
                    }
                case CreationResult.FalseCurrentUser:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "P�ihl�en� u�ivatel nem� platnou identifikaci.");
                        return RedirectToPage();
                    }
                case CreationResult.EnrollmentDuplicity:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "P�ihl�ka ji� existuje.");
                        return RedirectToPage();
                    }
                default:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "P�ihl�ku se nepoda�ilo vytvo�it z nezn�m�ho d�vodu.");
                        return RedirectToPage();
                    }
            }
            
        }
    }
}
