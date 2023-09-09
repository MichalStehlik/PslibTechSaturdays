using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using PslibTechSaturdays.Components;
using PslibTechSaturdays.ViewModels;

namespace PslibTechSaturdays.Areas.Admin.Pages.Actions
{
    public class IndexModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }
        public IndexModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public PaginatedList<ActionListVM> Actions { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public ActionsOrder Sort { get; set; } = ActionsOrder.Id;
        [BindProperty(SupportsGet = true)]
        public int? PageIndex { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Name { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? Active { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? Published { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Actions != null)
            {
                Action = await _context.Actions
                .Include(a => a.CreatedBy).ToListAsync();
                IQueryable<Models.Action> actions = _context.Actions
                    .Include(x => x.Groups)
                    .ThenInclude(g => g.Enrollments)
                    .AsQueryable();
                if (!String.IsNullOrEmpty(Name))
                    actions = actions.Where(i => (i.Name!.Contains(Name)));
                if (Year is not null)
                    actions = actions.Where(i => (i.Year == Year));
                if (Active is not null)
                {
                    actions = actions.Where(u => u.Active == Active);
                }
                if (Published is not null)
                {
                    actions = actions.Where(u => u.Published == Published);
                }

                Actions = await PaginatedList<ActionListVM>.CreateAsync(
                    actions.Select(x => new ActionListVM
                    {
                        ActionId = x.ActionId,
                        Name = x.Name,
                        Year = x.Year,
                        Created = x.Created,
                        Active = x.Active,
                        Published = x.Published,
                        GroupsCount = x.Groups!.Count()
                    }),
                    PageIndex ?? 1, PageSize ?? 100
                );
            }
        }
    }
    public enum ActionsOrder
    {
        Id,
        IdDesc,
        Name,
        NameDesc,
        Created,
        CreatedDesc,
        Year,
        YearDesc
    }
}
