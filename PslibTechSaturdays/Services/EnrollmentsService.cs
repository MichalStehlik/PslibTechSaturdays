using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.ViewModels;

namespace PslibTechSaturdays.Services
{
    public class EnrollmentsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EnrollmentsService> _logger;
        public IList<EnrollmentListVM> Enrollments { get; set; }

        public EnrollmentsService(ApplicationDbContext context, ILogger<EnrollmentsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<EnrollmentListVM> Items()
        {
            IQueryable<Enrollment> enr = _context.Enrollments.AsQueryable();
            var lst = enr.Select(x => new EnrollmentListVM
            {

            });
            return (lst.ToList());
        }
    }
    public enum EnrollmentsOrder
    {
        Id,
        IdDesc,
        UserName,
        UserNameDesc,
        GroupName,
        GroupNameDesc,
        ActionYear,
        ActionYearDesc,
        Created,
        CreatedDesc
    }
}
