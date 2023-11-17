using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Helpers;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Actions
{
    public class DeleteModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public DeleteModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Actions == null)
            {
                return NotFound();
            }
            var action = await _context.Actions.FindAsync(id);

            if (action != null)
            {
                try 
                {
                    Action = action;
                    _context.Actions.Remove(Action);
                    await _context.SaveChangesAsync();
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Akce byla odstraněna.");
                } 
                catch
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Akci se nepodařilo odstranit.");
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
