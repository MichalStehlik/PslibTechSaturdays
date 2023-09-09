using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models.Security;
using Microsoft.Graph.Models;
using PslibTechSaturdays.Constants;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Emails.PageModels;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.ViewModels;
using Microsoft.Data.SqlClient;
using PslibTechSaturdays.Components;

namespace PslibTechSaturdays.Areas.Admin.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        [TempData]
        public string? SuccessMessage { get; set; }
        [TempData]
        public string? FailureMessage { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginatedList<UserListVM> Users { get; set; } = default!;
        public UsersOrder Sort { get; set; } = UsersOrder.Id;
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public async Task OnGetAsync(
            string? search,
            string? email,
            string? userName,
            string? firstName,
            string? lastName,
            bool? admin,
            bool? lector,
            int? enrollments,
            UsersOrder? order,
            int? pageIndex,
            int pageSize = 10
            )
        {
            Sort = order ?? UsersOrder.Id;
            PageIndex = pageIndex;
            if (_context.Users != null)
            {
                IQueryable<ApplicationUser> users = _context.Users
                    .Include(x => x.Enrollments)
                .Include(x => x.Roles)
                    .Include(x => x.Certificates)
                    .AsQueryable();
                if (!String.IsNullOrEmpty(search))
                    users = users.Where(i => (i.UserName!.Contains(search)));
                if (!String.IsNullOrEmpty(email))
                    users = users.Where(i => (i.Email!.Contains(email)));
                if (!String.IsNullOrEmpty(userName))
                    users = users.Where(i => (i.UserName!.Contains(userName)));
                if (!String.IsNullOrEmpty(firstName))
                    users = users.Where(i => (i.FirstName.Contains(firstName)));
                if (enrollments is not null)
                    users = users.Where(i => (i.Enrollments!.Count > enrollments));
                if (!String.IsNullOrEmpty(lastName))
                    users = users.Where(i => (i.LastName.Contains(lastName)));
                if (admin is not null)
                {
                    if (admin == true)
                    {
                        users = users.Where(u => (u.Roles!.Any(x => x.NormalizedName == Constants.Security.ADMIN_ROLE.ToUpper())));
                    }
                    else
                    {
                        users = users.Where(u => (u.Roles!.All(x => x.NormalizedName != Constants.Security.ADMIN_ROLE.ToUpper())));
                    }
                };
                if (lector is not null)
                {
                    if (lector == true)
                    {
                        users = users.Where(u => (u.Roles!.Any(x => x.NormalizedName == Constants.Security.LECTOR_ROLE.ToUpper())));
                    }
                    else
                    {
                        users = users.Where(u => (u.Roles!.All(x => x.NormalizedName != Constants.Security.LECTOR_ROLE.ToUpper())));
                    }
                };

                users = order switch
                {
                    UsersOrder.Id => users.OrderBy(c => c.Id),
                    UsersOrder.IdDesc => users.OrderByDescending(c => c.Id),
                    UsersOrder.Email => users.OrderBy(c => c.Email),
                    UsersOrder.EmailDesc => users.OrderByDescending(c => c.Email),
                    UsersOrder.LastName => users.OrderBy(c => c.LastName),
                    UsersOrder.LastNameDesc => users.OrderByDescending(c => c.LastName),
                    UsersOrder.FirstName => users.OrderBy(c => c.FirstName),
                    UsersOrder.FirstNameDesc => users.OrderByDescending(c => c.FirstName),
                    UsersOrder.Created => users.OrderBy(c => c.Created),
                    UsersOrder.CreatedDesc => users.OrderByDescending(c => c.Created),
                    UsersOrder.Enrollments => users.OrderBy(c => c.Enrollments!.Count),
                    UsersOrder.EnrollmentsDesc => users.OrderByDescending(c => c.Enrollments!.Count),
                    _ => users
                };

                Users = await PaginatedList<UserListVM>.CreateAsync(
                    users.Select(x => new UserListVM
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        SchoolName = x.SchoolName,
                        Grade = x.Grade,
                        Email = x.Email,
                        BirthDate = x.BirthDate,
                        Created = x.Created,
                        Active = x.Active,
                        MailList = x.MailList,
                        Aspirant = x.Aspirant,
                        Roles = x.Roles,
                        Admin = x.Roles!.Any(x => x.NormalizedName == Constants.Security.ADMIN_ROLE.ToUpper()),
                        Lector = x.Roles!.Any(x => x.NormalizedName == Constants.Security.LECTOR_ROLE.ToUpper()),
                        EnrollmentsCount = x.Enrollments!.Count(),
                        CertificatesCount = x.Certificates!.Count(),
                    }),
                    pageIndex ?? 1, pageSize
                ); 
            }
        }
    }

    public enum UsersOrder {
        Id,
        IdDesc,
        FirstName,
        FirstNameDesc,
        LastName,
        LastNameDesc,
        Email,
        EmailDesc,
        Created,
        CreatedDesc,
        Enrollments,
        EnrollmentsDesc
    }
}
