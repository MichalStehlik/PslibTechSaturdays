using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<EditModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public EditInputModel Input { get; set; } = default!;
        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            Input = new EditInputModel 
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                SchoolName = user.SchoolName,
                Grade = user.Grade,
                Email = user.Email,
                Aspirant = user.Aspirant,
                MailList = user.MailList,
                PhoneNumber = user.PhoneNumber,
                Active = user.Active,
                TwoFactorEnabled = user.TwoFactorEnabled,
            };
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
            var user = await _userManager.FindByIdAsync(Input.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.BirthDate = Input.BirthDate;
            user.PhoneNumber = Input.PhoneNumber;
            user.Active = Input.Active;
            user.Aspirant = Input.Aspirant;
            user.Email = Input.Email;
            user.SchoolName = Input.SchoolName;
            user.Grade = Input.Grade;
            user.TwoFactorEnabled = Input.TwoFactorEnabled;
            user.Updated = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                SuccessMessage = "Uživatel byl aktualizován.";
                return RedirectToPage("./Index");
            }
            else
            {
                FailureMessage = "Při ukládání dat došlo k chybě.";
                return Page();
            }
        }

        private bool ApplicationUserExists(Guid id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

    public class EditInputModel
    {
        public Guid Id { get; set; }
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
        [Display(Name = "Aktivní")]
        public bool Active { get; set; }
        [Display(Name = "Zájemce o studium")]
        public bool Aspirant { get; set; }
        [Required]
        [Display(Name = "Odběratel zpráv")]
        public bool MailList { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; } = String.Empty;
        [Display(Name = "Telefon")]
        public string? PhoneNumber { get; set; } = String.Empty;
        [Display(Name = "Dvoufázové ověřování")]
        public bool TwoFactorEnabled { get; set; }
    }
}
