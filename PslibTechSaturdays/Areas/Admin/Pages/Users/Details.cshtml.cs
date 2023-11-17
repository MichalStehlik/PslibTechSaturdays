using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Helpers;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<DetailsModel> logger)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;

        }
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; } = default!;
        public List<ApplicationRole> AllRoles { get; set; } = default!;
        public List<string> Roles { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var applicationuser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (applicationuser == null)
            {
                return NotFound();
            }
            else
            {
                ApplicationUser = applicationuser;
                AllRoles = _roleManager.Roles.OrderBy(r => r.Name).ToList();
                Roles = (List<string>)await _userManager.GetRolesAsync(applicationuser);
                Id = applicationuser.Id;
            }
            return Page();
        }

        public async Task<IActionResult> OnGetAddRoleAsync(Guid? id, string name)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var applicationuser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (applicationuser == null)
            {
                return NotFound();
            }
            else
            {
                ApplicationUser = applicationuser;
                AllRoles = _roleManager.Roles.OrderBy(r => r.Name).ToList();
                Roles = (List<string>)await _userManager.GetRolesAsync(applicationuser);
                Id = applicationuser.Id;
            }
            var result = await _userManager.AddToRoleAsync(applicationuser,name);
            if (result.Succeeded)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Uživateli byla přiřazena role.");
            }
            else
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Přiřazení role se nepodařilo.");
            }
            return RedirectToPage("Details", new { Id = applicationuser.Id });
        }

        public async Task<IActionResult> OnGetRemoveRoleAsync(Guid? id, string name)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var applicationuser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (applicationuser == null)
            {
                return NotFound();
            }
            else
            {
                ApplicationUser = applicationuser;
                AllRoles = _roleManager.Roles.OrderBy(r => r.Name).ToList();
                Roles = (List<string>)await _userManager.GetRolesAsync(applicationuser);
                Id = applicationuser.Id;
            }
            var result = await _userManager.RemoveFromRoleAsync(applicationuser, name);
            if (result.Succeeded)
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Uživateli byla odebrána role.");
            }
            else
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Odebrání role se nepodařilo.");
            }
            return RedirectToPage("Details", new { Id = applicationuser.Id });
        }
    }
}
