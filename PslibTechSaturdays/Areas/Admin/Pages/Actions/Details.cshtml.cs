using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Helpers;

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
            catch {
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
    }
}
