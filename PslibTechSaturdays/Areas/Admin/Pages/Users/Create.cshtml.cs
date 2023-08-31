using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<CreateModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public CreateInputModel Input { get; set; } = default!;
        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Users == null || Input == null)
            {
                return Page();
            }

            ApplicationUser user = new ApplicationUser { 
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Email = Input.Email,
                UserName = Input.Email,
                SchoolName = Input.SchoolName,
                BirthDate = Input.BirthDate,
                Grade = Input.Grade,
                Aspirant = Input.Aspirant,
                MailList = Input.MailList,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                Active = true
        };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                var userId = await _userManager.GetUserIdAsync(user);
                if (Input.Administrator)
                {
                    await _userManager.AddToRoleAsync(user, Constants.Security.ADMIN_ROLE);
                }
                if (Input.Lector)
                {
                    await _userManager.AddToRoleAsync(user, Constants.Security.LECTOR_ROLE);
                }
                SuccessMessage = "Uživatel byl vytvořen";
                return RedirectToPage("./Index");
            }
            else
            {
                FailureMessage = "Při vytváření uživatele došlo k chybě.";
                return Page();
            }
        }
    }

    public class CreateInputModel
    {
        [Required]
        [Display(Name = "Jméno")]
        public string FirstName { get; set; } = String.Empty;
        [Required]
        [Display(Name = "Příjmení")]
        public string LastName { get; set; } = String.Empty;
        [Display(Name = "Datum narození")]
        public DateTime? BirthDate { get; set; }
        [Display(Name = "Název školy")]
        public string? SchoolName { get; set; }
        [Display(Name = "Třída")]
        public SchoolGrade Grade { get; set; }
        [Display(Name = "Zájemce o studium")]
        public bool Aspirant { get; set; }
        [Required]
        [Display(Name = "Odběratel zpráv")]
        public bool MailList { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = String.Empty;
        [Required(ErrorMessage = "Heslo musí být zadáno.")]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = String.Empty;
        [Display(Name = "Administrátor")]
        public bool Administrator { get; set; }
        [Display(Name = "Lektor")]
        public bool Lector { get; set; }
    }
}
