using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
                IQueryable<Models.Action> actions = _context.Actions
                    .Include(x => x.Groups)
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

                actions = Sort switch
                {
                    ActionsOrder.Id => actions.OrderBy(c => c.ActionId),
                    ActionsOrder.IdDesc => actions.OrderByDescending(c => c.ActionId),
                    ActionsOrder.Name => actions.OrderBy(c => c.Name),
                    ActionsOrder.NameDesc => actions.OrderByDescending(c => c.Name),
                    ActionsOrder.Year => actions.OrderBy(c => c.Year),
                    ActionsOrder.YearDesc => actions.OrderByDescending(c => c.Year),
                    ActionsOrder.Created => actions.OrderBy(c => c.Created),
                    ActionsOrder.CreatedDesc => actions.OrderByDescending(c => c.Created),
                    _ => actions
                };

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
