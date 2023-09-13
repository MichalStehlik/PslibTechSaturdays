using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Drives.Item.Items.Item.SearchWithQ;
using Microsoft.Graph.Models;
using PslibTechSaturdays.Areas.Admin.Pages.Users;
using PslibTechSaturdays.Components;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.ViewModels;

namespace PslibTechSaturdays.Areas.Admin.Pages.Groups
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public IndexModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SelectListItem> Actions { get; set; } = default!;
        public PaginatedList<GroupListVM> Groups { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public GroupsOrder Sort { get; set; } = GroupsOrder.Id;
        [BindProperty(SupportsGet = true)]
        public int? PageIndex { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Name { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? ActionId { get; set; }
        [BindProperty(SupportsGet = true)]
        public GroupState? State { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Actions == null)
            {
                return NotFound();
            }
            List<Models.Action> actions = _context.Actions.OrderByDescending(x => x.Start).ToList();
            Actions = new List<SelectListItem>();
            foreach(var actItem in actions)
            {
                Actions.Add(new SelectListItem { Value = actItem.ActionId.ToString(), Text = actItem.Year + " \\ " + actItem.Name});
            }
            if (_context.Groups != null)
            {
                IQueryable<Models.Group> groups = _context.Groups
                    .Include(x => x.Enrollments)
                    .Include(x => x.Action)
                    .Include(x => x.Lectors)
                    .AsQueryable();
                if (!String.IsNullOrEmpty(Name))
                    groups = groups.Where(i => (i.Name!.Contains(Name)));
                if (ActionId is not null)
                    groups = groups.Where(i => (i.ActionId == ActionId));
                if (Year is not null)
                    groups = groups.Where(i => (i.Action!.Year == Year));
                if (State is not null)
                {
                    switch (State)
                    {
                        case GroupState.Fresh: groups = groups = groups.Where(i => (i.PlannedOpening < DateTime.Now)); break;
                        case GroupState.Waiting: groups = groups = groups.Where(i => ((i.PlannedOpening > DateTime.Now) && (i.OpenedAt == null))); break;
                        case GroupState.Opened: groups = groups = groups.Where(i => ((i.OpenedAt < DateTime.Now) && (i.ClosedAt == null))); break;
                        case GroupState.Closed: groups = groups = groups.Where(i => ((i.ClosedAt != null))); break;
                        default: break;
                    }
                }

                groups = Sort switch
                {
                    GroupsOrder.Id => groups.OrderBy(c => c.GroupId),
                    GroupsOrder.IdDesc => groups.OrderByDescending(c => c.GroupId),
                    GroupsOrder.Name => groups.OrderBy(c => c.Name),
                    GroupsOrder.NameDesc => groups.OrderByDescending(c => c.Name),
                    GroupsOrder.ActionName => groups.OrderBy(c => c.Action!.Name),
                    GroupsOrder.ActionNameDesc => groups.OrderByDescending(c => c.Action!.Name),
                    GroupsOrder.Year => groups.OrderBy(c => c.Action!.Year),
                    GroupsOrder.YearDesc => groups.OrderByDescending(c => c.Action!.Year),
                    _ => groups
                };

                Groups = await PaginatedList<GroupListVM>.CreateAsync(
                    groups.Select(x => new GroupListVM 
                    {
                        GroupId = x.GroupId,
                        Name = x.Name,
                        Action = x.Action,
                        Capacity = x.Capacity,
                        LectorsCount = x.Lectors!.Count(),
                        EnrollmentsCount = x.Enrollments!.Count(),
                        ParticipantsCount = 0,
                        State = x.PlannedOpening > DateTime.Now
                            ? 
                            GroupState.Fresh 
                            :
                                (x.PlannedOpening < DateTime.Now) && (x.OpenedAt == null)    
                                ?
                                GroupState.Waiting
                                :
                                (x.OpenedAt != null) && (x.ClosedAt == null)
                                    ?
                                    GroupState.Opened
                                    :
                                    x.ClosedAt != null
                                        ?
                                        GroupState.Closed
                                        :
                                        GroupState.Errorneouns
                    }), PageIndex ?? 1, PageSize ?? 100
                );
            }
            return Page();
        }
    }

    public enum GroupsOrder
    {
        Id,
        IdDesc,
        Name,
        NameDesc,
        ActionName,
        ActionNameDesc,
        Year,
        YearDesc
    }
}
