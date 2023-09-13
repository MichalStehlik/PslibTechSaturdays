using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PslibTechSaturdays.Data;
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

        public IActionResult OnGet()
        {
        ViewData["ActionId"] = new SelectList(_context.Actions, "ActionId", "Name");
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "FirstName");
            return Page();
        }

        [BindProperty]
        public Group Group { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Groups == null || Group == null)
            {
                return Page();
            }

            _context.Groups.Add(Group);
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
        public int ActionId { get; set; }
        [DisplayName("Deklarovaná kapacita")]
        public int Capacity { get; set; }
        [DisplayName("Minimální třída")]
        public SchoolGrade MinGrade { get; set; } = SchoolGrade.None;
        [DisplayName("Veřejná poznámka")]
        public string? Note { get; set; }
        [DisplayName("Poznámka k lektorům")]
        public string? LectorsNote { get; set; }
        [JsonIgnore]
        public ICollection<ApplicationUser>? Lectors { get; set; }
        [DisplayName("Veřejný počet účastníků")]
        public bool ApplicationCountVisible { get; set; } = false;
        [Column(TypeName = "datetime2")]
        [DisplayName("Plánované otevření pro zápis")]
        public DateTime? PlannedOpening { get; set; }
    }
}
