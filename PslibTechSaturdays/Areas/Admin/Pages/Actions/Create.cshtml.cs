using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Helpers;

namespace PslibTechSaturdays.Areas.Admin.Pages.Actions
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(PslibTechSaturdays.Data.ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            Input = new CreateInputModel { ExclusiveEnrollment = true, Year = DateTime.Now.Year };
            return Page();
        }

        [BindProperty]
        public CreateInputModel Input { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Actions == null || Input == null)
            {
                return Page();
            }

            var userId = User!.Claims!.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            var action = new Models.Action
            {
                Name = Input.Name,
                Description = Input.Description,
                Year = Input.Year,
                Published = Input.Published,
                Start = Input.Start,
                End = Input.End,
                Created = DateTime.Now,
                CreatedById = Guid.Parse(userId),
                Active = true, 
                ExclusiveEnrollment = Input.ExclusiveEnrollment,
            };
            try
            {
                _context.Actions.Add(action);
                await _context.SaveChangesAsync();
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Akce byla vytvořena.");
                return RedirectToPage("./Index");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Při vytváření akce došlo k chybě.");
                return Page();
            }   
        }
    }

    public class CreateInputModel
    {
        [Required]
        [Display(Name = "Název")]
        public string Name { get; set; } = String.Empty;
        [Required]
        [Display(Name = "Popis")]
        public string Description { get; set; } = String.Empty;

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
