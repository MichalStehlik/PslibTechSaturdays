using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Constants;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.ViewModels;

namespace PslibTechSaturdays.Areas.Users.Pages
{
    [Authorize(Roles = Security.ADMIN_POLICY)]
    public class IndexModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string FailureMessage { get; set; }

        public IndexModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<UserListVM> Users { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Users != null)
            {
                IQueryable<ApplicationUser> users = _context.Users;
                Users = await users
                    .Include(x => x.Enrollments)
                    .Include(x => x.Roles)
                    .Include(x => x.Certificates)
                    .Select(x => new UserListVM { 
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
                        EnrollmentsCount = x.Enrollments.Count(),
                        CertificatesCount = x.Certificates.Count(),
                    })
                    .ToListAsync();
            }
        }
    }
}
