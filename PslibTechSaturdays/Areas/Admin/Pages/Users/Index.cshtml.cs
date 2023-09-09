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
        [BindProperty(SupportsGet = true)]
        public UsersOrder Sort { get; set; } = UsersOrder.Id;
        [BindProperty(SupportsGet = true)]
        public int? PageIndex { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? FirstName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? LastName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Email { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? Admin { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? Lector { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Users != null)
            {
                IQueryable<ApplicationUser> users = _context.Users
                    .Include(x => x.Enrollments)
                .Include(x => x.Roles)
                    .Include(x => x.Certificates)
                    .AsQueryable();

                if (!String.IsNullOrEmpty(Email))
                    users = users.Where(i => (i.Email!.Contains(Email)));
                if (!String.IsNullOrEmpty(FirstName))
                    users = users.Where(i => (i.FirstName.Contains(FirstName)));
                if (!String.IsNullOrEmpty(LastName))
                    users = users.Where(i => (i.LastName.Contains(LastName)));
                if (Admin is not null)
                {
                    if (Admin == true)
                    {
                        users = users.Where(u => (u.Roles!.Any(x => x.NormalizedName == Constants.Security.ADMIN_ROLE.ToUpper())));
                    }
                    else
                    {
                        users = users.Where(u => (u.Roles!.All(x => x.NormalizedName != Constants.Security.ADMIN_ROLE.ToUpper())));
                    }
                };
                if (Lector is not null)
                {
                    if (Lector == true)
                    {
                        users = users.Where(u => (u.Roles!.Any(x => x.NormalizedName == Constants.Security.LECTOR_ROLE.ToUpper())));
                    }
                    else
                    {
                        users = users.Where(u => (u.Roles!.All(x => x.NormalizedName != Constants.Security.LECTOR_ROLE.ToUpper())));
                    }
                };

                users = Sort switch
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
                    PageIndex ?? 1, PageSize ?? 100
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
