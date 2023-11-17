using Elfie.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Areas.Admin.Pages.Groups;
using PslibTechSaturdays.Components;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Pages;
using PslibTechSaturdays.Services;
using PslibTechSaturdays.ViewModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace PslibTechSaturdays.Areas.Admin.Pages.Enrollments
{
    public class IndexModel : PageModel
    {
        private readonly EnrollmentsService _storage;
        private readonly ILogger<IndexModel> _logger;
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public IndexModel(EnrollmentsService storage, ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _storage = storage;
            _logger = logger;
            _context = context;
        }

        public PaginatedList<EnrollmentListVM> Enrollments { get; set; } = default!;
        public List<SelectListItem> Groups { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Actions { get; set; } = new List<SelectListItem>();

        [BindProperty(SupportsGet = true)]
        public EnrollmentsOrder Sort { get; set; } = EnrollmentsOrder.Created;
        [BindProperty(SupportsGet = true)]
        public int? PageIndex { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }
        [BindProperty(SupportsGet = true)]
        [DisplayName("Jméno uživatele")]
        public string? UserName { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid? UserId { get; set; }
        [BindProperty(SupportsGet = true)]
        [DisplayName("Skupina")]
        public int? GroupId { get; set; }
        [BindProperty(SupportsGet = true)]
        [DisplayName("Akce")]
        public int? ActionId { get; set; }

        public async Task OnGetAsync()
        {
            var enr = await _storage.ItemsAsync(UserId, UserName, GroupId, ActionId, Sort, PageIndex, PageSize);
            foreach (var act in _context.Actions.OrderBy(a => a.Created))
            {
                var actionGroup = new SelectListGroup { Name = act.Name };
                foreach (var grp in _context.Groups.Where(g => g.ActionId == act.ActionId).OrderBy(g => g.Name))
                {
                    Groups.Add(new SelectListItem { Value = grp.GroupId.ToString(), Text = grp.Name, Group = actionGroup });
                }
            }
            foreach (var act in _context.Actions.OrderBy(a => a.Created))
            {
                Actions.Add(new SelectListItem { Value = act.ActionId.ToString(), Text = act.Name + " (" + act.Year + ")" });
            }
            var count = enr.Count();
            Enrollments = new PaginatedList<EnrollmentListVM>(enr, count, PageIndex ?? 1, PageSize ?? 100);
        }
    }
}
