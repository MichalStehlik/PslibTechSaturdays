using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Helpers;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Services;
using System.Security.Claims;

namespace PslibTechSaturdays.Areas.Lectoring.Pages.Groups
{
    public class DetailsModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;
        private readonly EnrollmentsService _es;

        public DetailsModel(ApplicationDbContext context, ILogger<DetailsModel> logger, EnrollmentsService es)
        {
            _context = context;
            _logger = logger;
            _es = es;
        }

        public Group? Group { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            Group = await _context.Groups!.FirstOrDefaultAsync(m => m.GroupId == id);
            if (Group == null)
            {
                return NotFound();
            }
            _context.Entry(Group).Reference(p => p.Action).Load();
            _context.Entry(Group).Collection(p => p.Enrollments!).Load();
            Enrollments = await _context.Enrollments.Include(e => e.User).Where(e => e.GroupId == id).OrderBy(e => e.User.LastName).ThenBy(e => e.User.FirstName).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetCancelAsync(int id)
        {
            if (_context.Enrollments == null)
            {
                return NotFound();
            }
            Enrollment? enr = await _context.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentId == id);
            if (enr == null)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Takov� p�ihl�ka neexistuje.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            _context.Entry(enr).Reference(p => p.Group).Load();
            _context.Entry(enr.Group).Collection(p => p.Lectors).Load();
            var userId = User!.Claims!.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            if (enr.Group.Lectors!.All(x => x.Id != Guid.Parse(userId) ))
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nem�te pr�vo vyhazovat ��astn�ky.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            if (!await _es.CancelAsync(id,Guid.Parse(userId)))
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Vyhozen� ��astn�ka se nepoda�ilo.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "U�ivatel byl ze skupiny vyhozen.");
            return RedirectToPage("Details", new { id = enr!.GroupId });
        }

        public async Task<IActionResult> OnGetPresenceAsync(int id, Presence presence)
        {
            if (_context.Enrollments == null)
            {
                return NotFound();
            }
            Enrollment? enr = await _context.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentId == id);
            if (enr == null)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Takov� p�ihl�ka neexistuje.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            _context.Entry(enr).Reference(p => p.Group).Load();
            _context.Entry(enr.Group).Collection(p => p.Lectors).Load();
            var userId = User!.Claims!.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            if (enr.Group.Lectors!.All(x => x.Id != Guid.Parse(userId)))
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nem�te pr�vo nastavovat p��tomnost ��astn�k�.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            if (!await _es.SetPresenceAsync(id, presence))
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nastaven� p��tomnosti se nepoda�ilo.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "P��tomnost u�ivatele byla nastavena.");
            return RedirectToPage("Details", new { id = enr!.GroupId });
        }
    }
}
