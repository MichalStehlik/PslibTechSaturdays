using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Helpers;

namespace PslibTechSaturdays.Areas.Admin.Pages.Actions
{
    public class EditModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;

        public EditModel(PslibTechSaturdays.Data.ApplicationDbContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public EditInputModel Input { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Actions == null)
            {
                return NotFound();
            }

            var action =  await _context.Actions.FirstOrDefaultAsync(m => m.ActionId == id);
            if (action == null)
            {
                return NotFound();
            }

            Input = new EditInputModel
            {
                ActionId = action.ActionId,
                Name = action.Name,
                Description = action.Description,
                Year = action.Year,
                Active = action.Active,
                Published = action.Published,
                Start = action.Start,
                End = action.End,
                ExclusiveEnrollment = action.ExclusiveEnrollment
            };

           ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FirstName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var action = await _context.Actions.FindAsync(Input.ActionId);
            if (action == null)
            {
                return NotFound(ModelState);
            }

            action.Name = Input.Name;
            action.Description = Input.Description;
            action.Year = Input.Year;
            action.Active = Input.Active;
            action.Published = Input.Published;
            action.Start = Input.Start;
            action.End = Input.End;

            try
            {
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Akce byla aktializována.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActionExists(Input.ActionId))
                {
                    return NotFound();
                }
                else
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Aktualizace akce se nepodařila.");
                } 
            }
            return RedirectToPage("./Index");
        }

        private bool ActionExists(int id)
        {
          return (_context.Actions?.Any(e => e.ActionId == id)).GetValueOrDefault();
        }
    }

    public class EditInputModel
    {
        public int ActionId { get; set; }
        [Required]
        [Display(Name = "Název")]
        public string Name { get; set; } = String.Empty;
        [Required]
        [Display(Name = "Popis")]
        public string? Description { get; set; } = String.Empty;

        [DisplayName("Školní rok")]
        public int Year { get; set; }
        [DisplayName("Aktivní")]
        public bool Active { get; set; } = true;
        [DisplayName("Zveřejněná")]
        public bool Published { get; set; } = false;

        [Column(TypeName = "datetime2")]
        [DisplayName("Čas začátku")]
        public DateTime? Start { get; set; }
        [Column(TypeName = "datetime2")]
        [DisplayName("Čas konce")]
        public DateTime? End { get; set; }
        [DisplayName("Lze se zapsat jen do jedné skupiny")]
        public bool ExclusiveEnrollment { get; set; } = true;
    }
}
