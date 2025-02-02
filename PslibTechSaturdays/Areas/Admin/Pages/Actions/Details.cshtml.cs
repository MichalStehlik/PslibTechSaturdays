using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Helpers;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.ViewModels;
using System.Text;
using System.Text.RegularExpressions;

namespace PslibTechSaturdays.Areas.Admin.Pages.Actions
{
    public class DetailsModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public DetailsModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Models.Action Action { get; set; } = default!;
        public List<GroupWithLastEnrollmentVM> Groups { get; set; } = new List<GroupWithLastEnrollmentVM>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Actions == null)
            {
                return NotFound();
            }

            var action = await _context.Actions.FirstOrDefaultAsync(m => m.ActionId == id);
            if (action == null)
            {
                return NotFound();
            }
            else
            {
                _context.Entry(action).Reference(p => p.CreatedBy).Load();
                Groups = await _context.Groups
                    .Include(g => g.Enrollments)
                    .Select(g => new GroupWithLastEnrollmentVM
                    {
                        GroupId = g.GroupId,
                        ActionId = g.ActionId,
                        Name = g.Name,
                        Capacity = g.Capacity,
                        OpenedAt = g.OpenedAt,
                        ClosedAt = g.ClosedAt,
                        EnrollmentsCountVisible = g.EnrollmentsCountVisible,
                        Lectors = g.Lectors!.ToList(),
                        EnrollmentCount = g.Enrollments!.Count, // Celkový počet přihlášek
                        ActiveEnrollmentCount = g.Enrollments!.Count(e => e.Cancelled == null), // Počet nezrušených přihlášek
                        LastActiveEnrollment = g.Enrollments != null && g.Enrollments.Any(e => e.Cancelled == null)
                            ? g.Enrollments
                                .Where(e => e.Cancelled == null)
                                .OrderByDescending(e => e.Created)
                                .FirstOrDefault()
                                .Created
                            : (DateTime?)null
                    })
                    .Where(g => g.ActionId == id)
                    .OrderBy(g => g.Name)
                    .ToListAsync();
                Action = action;
            }
            return Page();
        }

        public async Task<IActionResult> OnGetSetActiveAsync(int? id, bool value)
        {
            if (id == null || _context.Actions == null)
            {
                return NotFound();
            }

            var action = await _context.Actions.FirstOrDefaultAsync(m => m.ActionId == id);
            if (action == null)
            {
                return NotFound();
            }
            else
            {
                Action = action;
            }
            action.Active = value;
            try
            {
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Akce byla změněna.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Změna stavu neproběhla.");
            }
            return RedirectToPage("Details", new { Id = action.ActionId });
        }

        public async Task<IActionResult> OnGetSetPublishedAsync(int? id, bool value)
        {
            if (id == null || _context.Actions == null)
            {
                return NotFound();
            }

            var action = await _context.Actions.FirstOrDefaultAsync(m => m.ActionId == id);
            if (action == null)
            {
                return NotFound();
            }
            else
            {
                Action = action;
            }
            action.Published = value;
            try
            {
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Akce byla změněna.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Změna stavu neproběhla.");
            }
            return RedirectToPage("Details", new { Id = action.ActionId });
        }

        public async Task<IActionResult> OnGetOpenGroupsAsync(int? id)
        {
            if (id == null || _context.Actions == null)
            {
                return NotFound();
            }
            var action = await _context.Actions.FirstOrDefaultAsync(m => m.ActionId == id);
            if (action == null)
            {
                return NotFound();
            }
            try
            {
                await _context.Groups.Where(gr => gr.ActionId == id && gr.OpenedAt == null).ExecuteUpdateAsync(setter => setter.SetProperty(gr => gr.OpenedAt, DateTime.Now));
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Skupiny byly otevřeny.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Při otevírání skupin došlo k chybě.");
            }
            return RedirectToPage("Details", new { Id = action.ActionId });
        }
        public async Task<IActionResult> OnGetCloseGroupsAsync(int? id)
        {
            if (id == null || _context.Actions == null)
            {
                return NotFound();
            }
            var action = await _context.Actions.FirstOrDefaultAsync(m => m.ActionId == id);
            if (action == null)
            {
                return NotFound();
            }
            try
            {
                await _context.Groups.Where(gr => gr.ActionId == id && gr.OpenedAt != null && gr.ClosedAt == null).ExecuteUpdateAsync(setter => setter.SetProperty(gr => gr.ClosedAt, DateTime.Now));
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Skupiny byly uzavřeny.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Při uzavírání skupin došlo k chybě.");
            }
            return RedirectToPage("Details", new { Id = action.ActionId });
        }

        public async Task<IActionResult> OnGetShowCountsGroupsAsync(int? id, bool value)
        {
            if (id == null || _context.Actions == null)
            {
                return NotFound();
            }
            var action = await _context.Actions.FirstOrDefaultAsync(m => m.ActionId == id);
            if (action == null)
            {
                return NotFound();
            }
            try
            {
                await _context.Groups.Where(gr => gr.ActionId == id).ExecuteUpdateAsync(setter => setter.SetProperty(gr => gr.EnrollmentsCountVisible, value));
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Změna nastavení proběhla.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Při změně nastavení skupin došlo k chybě.");
            }
            return RedirectToPage("Details", new { Id = action.ActionId });
        }

        public async Task<IActionResult> OnGetDownloadEnrolledList(int? id)
        {
            if (id == null || _context.Actions == null)
            {
                return NotFound();
            }
            var action = await _context.Actions.FirstOrDefaultAsync(m => m.ActionId == id);
            if (action == null)
            {
                return NotFound();
            }
            var enrollments = await _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Group)
                .Where(e => e.Group.ActionId == id)
                .Where(e => e.Cancelled == null)
                .OrderBy(e => e.Created)
                .ToListAsync();
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            {
                writer.WriteLine("Jméno;Příjmení;E-mail;Skupina;Vytvořeno");
                foreach (var enrollment in enrollments)
                {
                    writer.WriteLine($"{enrollment.User.FirstName};{enrollment.User.LastName};{enrollment.User.Email};{enrollment.Group.Name};{enrollment.Created}");
                }
            }
            string sanitizedActionName = Regex.Replace(action.Name, @"[^A-Za-z0-9_]", "");
            stream.Position = 0;
            return File(stream, "text/csv", $"{sanitizedActionName}_prihlasky.csv");
        }
    }
}
