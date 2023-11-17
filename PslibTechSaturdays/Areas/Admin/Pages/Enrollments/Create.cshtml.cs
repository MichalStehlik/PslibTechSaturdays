using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models.ExternalConnectors;
using Microsoft.Graph.Models;
using System.Security.Claims;
using PslibTechSaturdays.Helpers;

namespace PslibTechSaturdays.Areas.Admin.Pages.Enrollments
{
    public class CreateModel : PageModel
    {
        private readonly EnrollmentsService _storage;
        private readonly ILogger<CreateModel> _logger;
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        public List<SelectListItem> ActiveGroups { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ActiveUsers { get; set; } = new List<SelectListItem>();

        public CreateModel(EnrollmentsService storage, ILogger<CreateModel> logger, ApplicationDbContext context)
        {
            _storage = storage;
            _logger = logger;
            _context = context;
        }

        public IActionResult OnGet()
        {
            foreach(var act in _context.Actions.Where(a => a.Active).OrderBy(a => a.Created))
            {
                var actionGroup = new SelectListGroup { Name = act.Name };
                foreach(var grp in _context.Groups.Where(g => g.ActionId == act.ActionId).OrderBy(g => g.Name))
                {
                    ActiveGroups.Add(new SelectListItem { Value = grp.GroupId.ToString(), Text = grp.Name, Group = actionGroup});
                }
            }
            foreach (var usr in _context.Users.Where(u => u.Active).OrderBy(g => g.LastName).ThenBy(g => g.FirstName))
            {
                ActiveUsers.Add(new SelectListItem { Value = usr.Id.ToString(), Text = usr.LastName + ", " + usr.FirstName + " (" + usr.Email + ")"});
            }
            Input.CheckOpened = true;
            Input.CheckCapacity = true;
            Input.CheckCondition = true;
            return Page();
        }

        [BindProperty]
        public CreateInputModel Input { get; set; } = new CreateInputModel();
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _storage == null || Input == null)
            {
                return Page();
            }

            var userId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var result = await _storage.CreateAsync(Input.UserId, Input.GroupId, userId, Input.CheckCapacity, Input.CheckOpened, Input.CheckCondition);
            switch(result)
            {
                case CreationResult.Success:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Přihláška byla vytvořená.");
                        return RedirectToPage("./Index");
                    }
                case CreationResult.FalseCurrentUser:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nemáte právo vytvářet takovou přihlášku.");
                        return RedirectToPage("./Index");
                    }
                case CreationResult.ExclusivityConflict:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Uživatel má přihlášku v jiné skupině.");
                        return RedirectToPage("./Index");
                    }
                case CreationResult.UnknownUser:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Uživatel neexistuje.");
                        return RedirectToPage("./Index");
                    }
                case CreationResult.FullCapacity:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Kapacita skupiny již je naplněná.");
                        return RedirectToPage("./Index");
                    }
                case CreationResult.ClosedGroup:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Skupina je uzavřená.");
                        return RedirectToPage("./Index");
                    }
                case CreationResult.SQLError:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Chyba SQL dotazu.");
                        return RedirectToPage("./Index");
                    }
                case CreationResult.ConditionUnsatisfied:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nejsou splněné podmínky zápisu.");
                        return RedirectToPage("./Index");
                    }
                case CreationResult.EnrollmentDuplicity:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Warning, "Uživatel už přihlášku má.");
                        return RedirectToPage("./Index");
                    }
                default:
                    {
                        TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Nespecifikovaná chyba při vytváření přihlášky.");
                        return Page();
                    }
            }

            
        }
    }

    public class CreateInputModel
    {
        [Required(ErrorMessage = "Skupina musí být vybrána")]
        [Display(Name = "Skupina")]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Uživatel musí být vybrán")]
        [Display(Name = "Uživatel")]
        public Guid UserId { get; set; }
        [Display(Name = "Kontrolovat, že je ve skupině volné místo")]
        public bool CheckCapacity { get; set; }
        [Display(Name = "Kontrolovat, že je ve skupina otevřená pro zápis")]
        public bool CheckOpened { get; set; }
        [Display(Name = "Kontrolovat, že uživatel splňuje podmínky")]
        public bool CheckCondition { get; set; }

    }
}
