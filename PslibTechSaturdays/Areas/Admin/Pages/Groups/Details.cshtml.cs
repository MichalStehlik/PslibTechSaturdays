using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Helpers;
using PslibTechSaturdays.Models;
using System.ComponentModel.DataAnnotations;

namespace PslibTechSaturdays.Areas.Admin.Pages.Groups
{
    public class DetailsModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public DetailsModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Group Group { get; set; } = default!;
        public List<SelectListItem> UnusedLectors { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> UnusedTags { get; set; } = new List<SelectListItem>();
        [Required(ErrorMessage = "Je potřeba nějakého lektora vybrat")]
        [BindProperty]
        public AddLectorInputModel InputLector { get; set; } = null!;
        [BindProperty]
        public AddTagInputModel InputTag { get; set; } = new AddTagInputModel();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }
            InputLector = new AddLectorInputModel {GroupId = (int)id };
            var lectors = _context.Users.Include(u => u.Roles)
                .Where(u => (u.Roles!.All(x => x.NormalizedName == Constants.Security.LECTOR_ROLE.ToUpper())))
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();
            UnusedLectors = new List<SelectListItem>();
            var tags = _context.Tags.OrderBy(t => t.Text).ToList();
            InputTag = new AddTagInputModel { GroupId = (int)id };
            UnusedTags = new List<SelectListItem>();
            var group = await _context.Groups.FirstOrDefaultAsync(m => m.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }
            else
            {
                _context.Entry(group).Reference(p => p.Action).Load();
                _context.Entry(group).Reference(p => p.CreatedBy).Load();
                _context.Entry(group).Collection(p => p.Lectors!).Load();
                _context.Entry(group).Collection(p => p.Tags!).Load();
                Group = group;
            }
            foreach (var l in lectors)
            {
                if(!group.Lectors!.Contains(l))
                {
                    UnusedLectors.Add(new SelectListItem { Value = l.Id.ToString(), Text = l.LastName + ", " + l.FirstName + " (" + l.Email + ")" });
                }
            }
            foreach (var t in tags)
            {
                if (!group.Tags!.Contains(t))
                {
                    UnusedTags.Add(new SelectListItem { Value = t.TagId.ToString(), Text = t.Text });
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostLectorAsync()
        {
                if (InputLector.UserId == null)
                {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Žádný lektor nebyl vybrán.");
                    return RedirectToPage("Details", new { id = InputLector.GroupId });
                }
                var grp = _context.Groups.FirstOrDefault(g => g.GroupId == InputLector.GroupId);
                if (grp == null) 
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Taková skupina neexistuje.");
                    return RedirectToPage("Details", new { id = InputLector.GroupId });
                }
                var usr = _context.Users.FirstOrDefault(u => u.Id == Guid.Parse(InputLector.UserId));
                if (usr == null)
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Takový uživatel neexistuje.");
                    return RedirectToPage("Details", new { id = InputLector.GroupId });
                }
                _context.Entry(grp).Collection(p => p.Lectors!).Load();
                if (grp!.Lectors!.Contains(usr))
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Lektor už v této skupině je.");
                    return RedirectToPage("Details", new { id = InputLector.GroupId });
                }
                grp!.Lectors!.Add(usr);
                try
                {
                    await _context.SaveChangesAsync();
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Lektor byl přidán.");
            }
                catch
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Přidání lektora se nepodařilo.");
            }        
                return RedirectToPage("Details", new { id = InputLector.GroupId });
        }

        public async Task<IActionResult> OnPostTagAsync()
        {
                if (InputTag.TagId == null)
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Žádná značka nebyla vybraná.");
                    return RedirectToPage("Details", new { id = InputTag.GroupId });
                }
                var grp = _context.Groups.FirstOrDefault(g => g.GroupId == InputTag.GroupId);
                if (grp == null)
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Skupina neexistuje.");
                    return RedirectToPage("Details", new { id = InputTag.GroupId });
                }
                var tag = _context.Tags.FirstOrDefault(t => t.TagId == InputTag.TagId);
                if (tag == null)
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Značka neexistuje.");
                    return RedirectToPage("Details", new { id = InputTag.GroupId });
                }
                _context.Entry(grp).Collection(p => p.Tags!).Load();
                if (grp!.Tags!.Contains(tag))
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Značka už ke skupině patří.");
                    return RedirectToPage("Details", new { id = InputTag.GroupId });
                }
                grp!.Tags!.Add(tag);
                try
                {
                    await _context.SaveChangesAsync();
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Značka byla přiřazena úspěšně.");
                }
                catch
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Přiřazení značky se nepodařilo.");
                }
                return RedirectToPage("Details", new { id = InputTag.GroupId });
        }

        public async Task<ActionResult> OnGetRemoveLectorAsync(int groupId, Guid userId)
        {
            var grp = _context.Groups.FirstOrDefault(g => g.GroupId == groupId);
            if (grp == null)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Taková skupina neexistuje.");
                return RedirectToPage("Index");
            }
            var usr = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (usr == null)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Takový uživatel neexistuje.");
                return RedirectToPage("Details", new { id = groupId });
            }
            _context.Entry(grp).Collection(p => p.Lectors!).Load();
            grp.Lectors!.Remove(usr);
            try
            {
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Lektor byl odebrán.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Odebrání lektora se nepodařilo.");
            }
            return RedirectToPage("Details", new { id = groupId });
        }

        public async Task<ActionResult> OnGetRemoveTagAsync(int groupId, int tagId)
        {
            var grp = _context.Groups.FirstOrDefault(g => g.GroupId == groupId);
            if (grp == null)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Skupina neexistuje.");
                return RedirectToPage("Index");
            }
            var tag = _context.Tags.FirstOrDefault(t => t.TagId == tagId);
            if (tag == null)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Značka neexistuje.");
                return RedirectToPage("Details", new { id = groupId });
            }
            _context.Entry(grp).Collection(p => p.Tags!).Load();
            grp.Tags!.Remove(tag);
            try
            {
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Značka byla odebrána.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Odebrání značky se nepodařilo.");
            }
            return RedirectToPage("Details", new { id = groupId });
        }

        public async Task<ActionResult> OnGetOpenAsync(int id)
        {
            var grp = _context.Groups.FirstOrDefault(g => g.GroupId == id);
            if (grp == null)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Skupina neexistuje.");
                return RedirectToPage("Index");
            }
            try
            {
                grp.OpenedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Skupina byla otevřena pro zápis.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Při otevírání skupiny došlo k chybě.");
            }
            return RedirectToPage("Details", new { id = id });
        }

        public async Task<ActionResult> OnGetCloseAsync(int id)
        {
            var grp = _context.Groups.FirstOrDefault(g => g.GroupId == id);
            if (grp == null)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Skupina neexistuje.");
                return RedirectToPage("Index");
            }
            try
            {
                grp.ClosedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Skupina byla uzavřena pro zápis.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Při uzavírání skupiny došlo k chybě.");
            }
            return RedirectToPage("Details", new { id = id });
        }

        public async Task<ActionResult> OnGetReopenAsync(int id)
        {
            var grp = _context.Groups.FirstOrDefault(g => g.GroupId == id);
            if (grp == null)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Skupina neexistuje.");
                return RedirectToPage("Index");
            }
            try
            {
                grp.ClosedAt = null;
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Skupina byla znovuotevřena pro zápis.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Při znovuotevírání skupiny došlo k chybě.");
            }
            return RedirectToPage("Details", new { id = id });
        }

        public async Task<ActionResult> OnGetSetCountVisibleAsync(int id, bool value)
        {
            var grp = _context.Groups.FirstOrDefault(g => g.GroupId == id);
            if (grp == null)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Skupina neexistuje.");
                return RedirectToPage("Index");
            }
            try
            {
                grp.EnrollmentsCountVisible = value;
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Nastavení skupiny bylo změněno.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Došlo k chybě při změně nastavení skupiny.");
            }
            return RedirectToPage("Details", new { id = id });
        }
    }

    public class AddLectorInputModel
    {
        [Required(ErrorMessage = "Skupina musí být vybrána.")]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Uživatel musí být vybrán.")]
        public string? UserId { get; set; }
    }
    public class AddTagInputModel
    {
        [Required(ErrorMessage = "Skupina musí být vybrána.")]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Značka musí být vybrána.")]
        public int? TagId { get; set; }
    }
}
