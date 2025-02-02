using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PslibTechSaturdays.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PslibTechSaturdays.Helpers;
using System.Security.Claims;

namespace PslibTechSaturdays.Areas.Lectoring.Pages.Groups
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

        public SelectList? Actions { get; set; }

        public IActionResult OnGet(int? action, int? source)
        {
            if (source != null)
            {
                var sourceGroup = _context.Groups.FirstOrDefault(g => g.GroupId == source);
                if (sourceGroup != null)
                {
                    Input = new CreateInputModel
                    {
                        Name = sourceGroup.Name,
                        Description = sourceGroup.Description,
                        ActionId = sourceGroup.ActionId,
                        Capacity = sourceGroup.Capacity,
                        MinGrade = sourceGroup.MinGrade,
                        Note = sourceGroup.Note,
                        LectorsNote = sourceGroup.LectorsNote,
                        ApplicationCountVisible = sourceGroup.EnrollmentsCountVisible,
                        PlannedOpening = sourceGroup.PlannedOpening
                    };
                }
                else
                {
                    TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Warning, "Zdrojová skupina nebyla nalezena.");
                    Input = new CreateInputModel();
                }
            }
            else
            {
                Input = new CreateInputModel();
                if (action == null)
                {
                    Input.ActionId = action;
                }
            }

            Actions = new SelectList(_context.Actions, "ActionId", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Actions = new SelectList(_context.Actions, "ActionId", "Name");
                return Page();
            }
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            var group = new Group
            {
                Name = Input.Name,
                Description = Input.Description,
                ActionId = Input.ActionId.Value,
                Capacity = Input.Capacity,
                MinGrade = Input.MinGrade,
                Note = Input.Note,
                LectorsNote = Input.LectorsNote,
                EnrollmentsCountVisible = Input.ApplicationCountVisible,
                PlannedOpening = Input.PlannedOpening,
                CreatedById = Guid.Parse(userId),
                Created = DateTime.Now
            };
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Group {0} created by {1}.", group.GroupId, userId);
            group.Lectors = new List<ApplicationUser>();
            var user = _context.Users.Find(Guid.Parse(userId));
            if (user != null)
            {
                group.Lectors.Add(user);
                await _context.SaveChangesAsync();
            }       
            TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Skupina byla úspìšnì vytvoøena.");

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
        [DisplayName("Minimální tøída")]
        public SchoolGrade MinGrade { get; set; } = SchoolGrade.None;
        [DisplayName("Veøejná poznámka")]
        public string? Note { get; set; }
        [DisplayName("Poznámka k lektorùm")]
        public string? LectorsNote { get; set; }
        [DisplayName("Veøejný poèet úèastníkù")]
        public bool ApplicationCountVisible { get; set; } = false;
        [Column(TypeName = "datetime2")]
        [DisplayName("Plánované otevøení pro zápis")]
        public DateTime? PlannedOpening { get; set; }
    }
}
