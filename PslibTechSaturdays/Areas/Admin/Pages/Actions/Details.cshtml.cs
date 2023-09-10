using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Actions
{
    public class DetailsModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }

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
                SuccessMessage = "Akce byla změněna.";
            }
            catch {
                FailureMessage = "Změna stavu neproběhla.";
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
                SuccessMessage = "Akce byla změněna.";
            }
            catch
            {
                FailureMessage = "Změna stavu neproběhla.";
            }
            return RedirectToPage("Details", new { Id = action.ActionId });
        }
    }
}
