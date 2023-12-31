﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Helpers;
using PslibTechSaturdays.Models;

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
        public List<Group> Groups { get; set; } = new List<Group>();

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
                Groups = await _context.Groups.Include(g => g.Lectors).Include(g => g.Enrollments).Where(g => g.ActionId == id).OrderBy(g => g.Name).ToListAsync();
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
    }
}
