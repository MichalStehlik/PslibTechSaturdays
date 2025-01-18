using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using System.ComponentModel.DataAnnotations;
using PslibTechSaturdays.Helpers;

namespace PslibTechSaturdays.Areas.Admin.Pages.Groups
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
        public SelectList? Actions { get; set; }
        [BindProperty]
        public EditInputModel Input { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group =  await _context.Groups.FirstOrDefaultAsync(m => m.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }

            Input = new EditInputModel 
            {
                GroupId = group.GroupId,
                Name = group.Name,
                Description = group.Description ?? "",
                ActionId = group.ActionId,
                Capacity = group.Capacity,
                MinGrade = group.MinGrade,
                Note = group.Note ?? "",
                LectorsNote = group.LectorsNote ?? "",
                EnrollmentsCountVisible = group.EnrollmentsCountVisible,
                PlannedOpening = group.PlannedOpening,
                OpenedAt = group.OpenedAt,
                ClosedAt = group.ClosedAt
            };

            Actions = new SelectList(_context.Actions, "ActionId", "Name");
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

            var group = await _context.Groups.FindAsync(Input.GroupId);
            if (group == null)
            {
                return NotFound(ModelState);
            }
            group.Name = Input.Name;
            group.Description = Input.Description;
            group.ActionId = (int)Input.ActionId;
            group.Note = Input.Note;
            group.Capacity = Input.Capacity;
            group.MinGrade = Input.MinGrade;
            group.LectorsNote = Input.LectorsNote;
            group.EnrollmentsCountVisible = Input.EnrollmentsCountVisible;
            group.PlannedOpening = Input.PlannedOpening;
            group.OpenedAt = Input.OpenedAt;
            group.ClosedAt = Input.ClosedAt;

            try
            {
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Skupina byla aktualizována.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(Input.GroupId))
                {
                    return NotFound();
                }
                else
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Aktualizace skupiny se nepodařila.");
                }
            }

            return RedirectToPage("./Index");
        }

        private bool GroupExists(int id)
        {
          return (_context.Groups?.Any(e => e.GroupId == id)).GetValueOrDefault();
        }
    }

    public class EditInputModel
    {
        public int GroupId { get; set; }
        [Required]
        [DisplayName("Název")]
        public string Name { get; set; } = String.Empty;
        [DisplayName("Popis")]
        public string? Description { get; set; } = String.Empty;
        [Required]
        [DisplayName("Akce")]
        public int? ActionId { get; set; }
        [DisplayName("Deklarovaná kapacita")]
        public int Capacity { get; set; }
        [DisplayName("Minimální třída")]
        public SchoolGrade MinGrade { get; set; } = SchoolGrade.None;
        [DisplayName("Veřejná poznámka")]
        public string? Note { get; set; } = String.Empty;
        [DisplayName("Poznámka k lektorům")]
        public string? LectorsNote { get; set; } = String.Empty;
        [DisplayName("Veřejný počet účastníků")]
        public bool EnrollmentsCountVisible { get; set; } = false;
        [Column(TypeName = "datetime2")]
        [DisplayName("Plánované otevření pro zápis")]
        public DateTime? PlannedOpening { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
