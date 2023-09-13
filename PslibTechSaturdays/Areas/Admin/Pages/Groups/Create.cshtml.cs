using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Groups
{
    public class CreateModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(PslibTechSaturdays.Data.ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public CreateInputModel Input { get; set; } = default!;
        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }

        public SelectList? Actions { get; set; }

        public IActionResult OnGet(int? action)
        {
            Input = new CreateInputModel();
            if (action == null)
            {
                Input.ActionId = action;
            }
            Actions = new SelectList(_context.Actions, "ActionId", "Name");
            return Page();
        }        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Groups == null || Input == null)
            {
                return Page();
            }
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var group = new Group
            {
                Name = Input.Name,
                Description = Input.Description,
                ActionId = (int)Input.ActionId,
                Capacity = Input.Capacity,
                MinGrade = Input.MinGrade,
                Note = Input.Note,
                LectorsNote = Input.LectorsNote,
                ApplicationCountVisible = Input.ApplicationCountVisible,
                PlannedOpening = Input.PlannedOpening, 
                CreatedById = Guid.Parse(userId),
                Created = DateTime.Now,
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }

    public class CreateInputModel
    {
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
        public string? Note { get; set; }
        [DisplayName("Poznámka k lektorům")]
        public string? LectorsNote { get; set; }
        [DisplayName("Veřejný počet účastníků")]
        public bool ApplicationCountVisible { get; set; } = false;
        [Column(TypeName = "datetime2")]
        [DisplayName("Plánované otevření pro zápis")]
        public DateTime? PlannedOpening { get; set; }
    }
}
