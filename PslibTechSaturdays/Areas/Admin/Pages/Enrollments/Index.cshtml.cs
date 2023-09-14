using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Services;
using PslibTechSaturdays.ViewModels;

namespace PslibTechSaturdays.Areas.Admin.Pages.Enrollments
{
    public class IndexModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        private readonly EnrollmentsService _storage;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Enrollment> Enrollment { get;set; } = default!;
        public IList<EnrollmentListVM> Enrollments { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Enrollments != null)
            {
                Enrollment = await _context.Enrollments
                .Include(e => e.CancelledBy)
                .Include(e => e.Certificate)
                .Include(e => e.CreatedBy)
                .Include(e => e.Group)
                .Include(e => e.User).ToListAsync();
            }
        }
    }
    public enum EnrollmentsOrder
    {
        Id,
        IdDesc,
        GroupName,
        GroupNameDesc,
        ActionYear,
        ActionYearDesc,
        Created,
        CreatedDesc
    }
}
