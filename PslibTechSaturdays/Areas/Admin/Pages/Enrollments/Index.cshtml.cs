using Elfie.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Areas.Admin.Pages.Groups;
using PslibTechSaturdays.Components;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Pages;
using PslibTechSaturdays.Services;
using PslibTechSaturdays.ViewModels;
using System.Drawing.Printing;

namespace PslibTechSaturdays.Areas.Admin.Pages.Enrollments
{
    public class IndexModel : PageModel
    {
        private readonly EnrollmentsService _storage;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(EnrollmentsService storage, ILogger<IndexModel> logger)
        {
            _storage = storage;
            _logger = logger;
        }

        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }

        public PaginatedList<EnrollmentListVM> Enrollments { get; set; } = default!;

        public List<SelectListItem> Actions { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public EnrollmentsOrder Sort { get; set; } = EnrollmentsOrder.Created;
        [BindProperty(SupportsGet = true)]
        public int? PageIndex { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? UserName { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid? UserId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? GroupId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? ActionId { get; set; }

        public async Task OnGetAsync()
        {
            var enr = await _storage.ItemsAsync();
            var count = enr.Count();
            Enrollments = new PaginatedList<EnrollmentListVM>(enr, count, PageIndex ?? 1, PageSize ?? 100);
        }
    }
}
