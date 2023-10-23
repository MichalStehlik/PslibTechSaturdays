using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Certificates
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public CreateModel(ILogger<CreateModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult OnGet()
        {
            Input = new CreateInputModel { Title = "Osvědčení"};
            var users = _context.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();
            foreach(var u in users)
            {
                Users.Add(new SelectListItem { Value = u.Id.ToString(), Text = u.LastName + ", " + u.FirstName + " (" + u.Email + ")"});
            }
            return Page();
        }

        [BindProperty]
        public CreateInputModel Input { get; set; } = default!;
        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }

        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Certificates == null || Input == null)
            {
                return Page();
            }

            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value;
            _context.Certificates.Add( new Certificate
            {
                Title = Input.Title,
                Description = Input.Description,
                UserId = Input.UserId,
                CreatedById = Guid.Parse(userId),
                Issued = DateTime.Now,
            });
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }

    public class CreateInputModel
    {
        [Required(ErrorMessage = "Uživatel musí být vybrán")]
        [Display(Name = "Uživatel")]
        public Guid UserId { get; set; }
        [Display(Name = "Titulek")]
        [Required(ErrorMessage = "Certifikát musí mít název")]
        public string Title { get; set; } = String.Empty;

        [DisplayName("Popisek")]
        public string Description { get; set; } = String.Empty;
    }
}
