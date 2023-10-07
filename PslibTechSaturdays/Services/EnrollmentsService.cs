using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Areas.Admin.Pages.Groups;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.ViewModels;

namespace PslibTechSaturdays.Services
{
    public class EnrollmentsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EnrollmentsService> _logger;

        public EnrollmentsService(ApplicationDbContext context, ILogger<EnrollmentsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<EnrollmentListVM>> ItemsAsync(Guid? userId = null, string? name = null, int? groupId = null, int? actionId = null, EnrollmentsOrder? sort = EnrollmentsOrder.Created, int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<Enrollment> enr = _context.Enrollments
                .Include(e => e.CancelledBy)
                .Include(e => e.Certificate)
                .Include(e => e.CreatedBy)
                .Include(e => e.Group).ThenInclude(g => g.Action)
                .Include(e => e.User).AsQueryable();
            if (userId != null)
                enr = enr.Where(i => (i.User.Id == userId));
            if (!String.IsNullOrEmpty(name))
                enr = enr.Where(i => (i.User.FirstName.Contains(name) || (i.User.LastName.Contains(name))));
            if (groupId != null)
                enr = enr.Where(i => (i.GroupId == groupId));
            if (actionId != null)
                enr = enr.Where(i => (i.Group.ActionId == actionId));
            enr = sort switch
            {
                EnrollmentsOrder.Id => enr.OrderBy(c => c.GroupId),
                EnrollmentsOrder.IdDesc => enr.OrderByDescending(c => c.GroupId),
                EnrollmentsOrder.UserName => enr.OrderBy(c => c.User.LastName),
                EnrollmentsOrder.UserNameDesc => enr.OrderByDescending(c => c.User.LastName),
                EnrollmentsOrder.GroupName => enr.OrderBy(c => c.Group!.Name),
                EnrollmentsOrder.GroupNameDesc => enr.OrderByDescending(c => c.Group!.Name),
                EnrollmentsOrder.Created => enr.OrderBy(c => c.Created),
                EnrollmentsOrder.CreatedDesc => enr.OrderByDescending(c => c.Created),
                _ => enr
            };
            var lst = enr.Select(x => new EnrollmentListVM
            {
                EnrollmentId = x.EnrollmentId,
                Group = x.Group,
                Action = x.Group!.Action,
                User = x.User,
                CreatedBy = x.CreatedBy,
                Created = x.Created,
                Cancelled = x.Cancelled,
                CancelledBy = x.CancelledBy,
                Present = x.Present,
                Certificate = x.Certificate
            });
            return (await lst.ToListAsync());
        }

        public async Task<Enrollment?> GetAsync(int id)
        {
            var enrollment = await _context.Enrollments.Where(e => e.EnrollmentId == id).SingleOrDefaultAsync();
            if (enrollment != null)
            {
                _context.Entry(enrollment).Reference(e => e.User).Load();
                _context.Entry(enrollment).Reference(e => e.CreatedBy).Load();
                _context.Entry(enrollment).Reference(e => e.CancelledBy).Load();
                _context.Entry(enrollment).Reference(e => e.Group).Load();
            }        
            return enrollment;
        }

        public async Task<CreationResult> CreateAsync(Guid userId, int groupId, Guid currentUserId, bool checkCapacity = true, bool checkOpened = true, bool checkCondition = true)
        {
            var user = await _context.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
            if (user == null)
            {
                return CreationResult.UnknownUser;
            }
            var creator = await _context.Users.Where(u => u.Id == currentUserId).SingleOrDefaultAsync();
            if (user == null)
            {
                return CreationResult.FalseCurrentUser;
            }
            var group = await _context.Groups.Where(u => u.GroupId == groupId).SingleOrDefaultAsync();
            if (group == null)
            {
                return CreationResult.UnknownGroup;
            }
            if (checkOpened)
            {
                if (group!.OpenedAt > DateTime.Now || group.ClosedAt != null)
                {
                    return CreationResult.ClosedGroup;
                }
            }
            _context.Entry(group).Collection(p => p.Enrollments!).Load();
            var total = group.Enrollments!.Count();
            var valid = group.Enrollments!.Where(x => x.Cancelled == null).Count();
            if (checkCapacity)
            {
                if (valid >= group.Capacity)
                {
                    return CreationResult.FullCapacity;
                }
            }
            var enrollment = new Enrollment
            {
                User = user,
                Group = group,
                Created = DateTime.Now,
                CreatedBy = creator,
                CancelledById = null,
            };
            try
            {
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
                return CreationResult.Success;
            }
            catch
            {
                return CreationResult.SQLError;
            }
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var enrollment = await _context.Enrollments.Where(e => e.EnrollmentId == id).SingleOrDefaultAsync();
            if (enrollment == null)
            {
                return false;
            }
            try
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelAsync(int id, Guid userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
            if (user == null)
            {
                return false;
            }
            var enrollment = await _context.Enrollments.Where(e => e.EnrollmentId == id).SingleOrDefaultAsync();
            if (enrollment == null)
            {
                return false;
            }
            try
            {
                enrollment.Cancelled = DateTime.Now;
                enrollment.CancelledBy = user;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SetPresenceAsync(int id, Presence state)
        {
            var enrollment = await _context.Enrollments.Where(e => e.EnrollmentId == id).SingleOrDefaultAsync();
            if (enrollment == null)
            {
                return false;
            }
            try
            {
                enrollment.Present = state;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
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
        Created,
        CreatedDesc
    }

    public enum CreationResult
    {
        Success,
        UnknownUser,
        UnknownGroup,
        FalseCurrentUser,
        FullCapacity,
        ExclusivityConflict,
        ClosedGroup,
        SQLError,
        ConditionUnsatisfied
    }
}
