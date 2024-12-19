using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Helpers;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Services;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace PslibTechSaturdays.Areas.Lectoring.Pages.Groups
{
    public class DetailsModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;
        private readonly EnrollmentsService _es;
        private readonly CertificateGenerationService _cgs;

        public DetailsModel(ApplicationDbContext context, ILogger<DetailsModel> logger, EnrollmentsService es, CertificateGenerationService cgs)
        {
            _context = context;
            _logger = logger;
            _es = es;
            _cgs = cgs;
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
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Taková pøihláška neexistuje.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            _context.Entry(enr).Reference(p => p.Group).Load();
            _context.Entry(enr.Group).Collection(p => p.Lectors!).Load();
            var userId = User!.Claims!.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            if (enr.Group.Lectors!.All(x => x.Id != Guid.Parse(userId) ))
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nemáte právo vyhazovat úèastníky.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            if (!await _es.CancelAsync(id,Guid.Parse(userId)))
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Vyhození úèastníka se nepodaøilo.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Uživatel byl ze skupiny vyhozen.");
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
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Taková pøihláška neexistuje.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            _context.Entry(enr).Reference(p => p.Group).Load();
            _context.Entry(enr.Group).Collection(p => p.Lectors!).Load();
            var userId = User!.Claims!.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            if (enr.Group.Lectors!.All(x => x.Id != Guid.Parse(userId)))
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nemáte právo nastavovat pøítomnost úèastníkù.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            if (!await _es.SetPresenceAsync(id, presence))
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nastavení pøítomnosti se nepodaøilo.");
                return RedirectToPage("Details", new { id = enr!.GroupId });
            }
            TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Pøítomnost uživatele byla nastavena.");
            return RedirectToPage("Details", new { id = enr!.GroupId });
        }

        public async Task<IActionResult> OnGetCertPresentAsync(int id)
        {
            if (_context.Enrollments == null)
            {
                return NotFound();
            }
            List<Enrollment> enrollments = await _context.Enrollments
                .Include(e => e.Group)
                .ThenInclude(g => g.Action)
                .Where(e => e.GroupId == id)
                .Where(e => e.CertificateId == null)
                .Where(e => e.Present == Presence.Present)
                .ToListAsync();
            var userId = User!.Claims!.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            if (userId == null)
            {
                return NotFound();
            }
            foreach (var en in enrollments)
            {
                DateTime? e = en.Group.Action!.End;
                StringBuilder sb = new StringBuilder("Úèastník absolvoval");
                if (e != null) sb.Append(" dne " + ((DateTime)e).ToString("d", new CultureInfo("cs-CZ")));
                sb.Append(" akci " + en.Group.Action.Name);
                var cert = new Certificate
                {
                    CreatedById = Guid.Parse(userId),
                    Title = "Osvìdèení",
                    Text = sb.ToString(),
                    Description = en.Group.Name,
                    UserId = en.ApplicationUserId,
                    Issued = DateTime.Now,
                };
                try
                {
                    _context.Certificates.Add(cert);
                    await _context.SaveChangesAsync();
                    en.CertificateId = cert.CertificateId;
                    await _context.SaveChangesAsync();
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Osvìdèení " + cert.CertificateId.ToString() + " bylo udìleno.");
                }
                catch
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Pøi vydávání osvìdèení došlo k chybì.");
                }               
            }        
            return RedirectToPage("Details", new { id = id });
        }

        public async Task<ActionResult> OnGetDownloadHtmlAsync(Guid id)
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            Certificate? cert = await _context.Certificates.FirstOrDefaultAsync(x => x.CertificateId == id);
            if (cert == null)
            {
                return NotFound();
            }
            _context.Entry(cert).Reference(p => p.User).Load();
            string html = await _cgs.GetHtmlAsync(cert);
            return new ContentResult { Content = html, ContentType = "text/html" };
        }
    }
}
