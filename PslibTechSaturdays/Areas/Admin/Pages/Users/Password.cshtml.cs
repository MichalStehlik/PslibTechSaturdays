using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using System.ComponentModel.DataAnnotations;

namespace PslibTechSaturdays.Areas.Admin.Pages.Users
{
    public class PasswordModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<EditModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public PasswordInputModel Input { get; set; } = default!;
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
            Input = new PasswordInputModel { Id = (Guid)id };
            return Page();
        }

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

            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user,Input.Password);
            if (result.Succeeded)
            {
                SuccessMessage = "Heslo bylo aktualizována.";
                return RedirectToPage("./Details", new { Id = Input.Id});
            }
            else
            {
                FailureMessage = "Pøi ukládání hesla došlo k chybì.";
                return Page();
            }
        }
    }

    public class PasswordInputModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = String.Empty;
    }
}
