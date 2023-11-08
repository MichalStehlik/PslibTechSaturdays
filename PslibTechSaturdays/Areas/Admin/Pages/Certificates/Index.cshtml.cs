using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Areas.Admin.Pages.Groups;
using PslibTechSaturdays.Components;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.ViewModels;

namespace PslibTechSaturdays.Areas.Admin.Pages.Certificates
{
    public class IndexModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public IndexModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public PaginatedList<CertificateListVM> Certificates { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public CertificatesOrder Sort { get; set; } = CertificatesOrder.Id;
        [BindProperty(SupportsGet = true)]
        public int? PageIndex { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Name { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid? UserId { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Certificates != null)
            {
                IQueryable<Certificate> certificates = _context.Certificates
                    .Include(x => x.User)
                    .Include(x => x.CreatedBy)
                    .AsQueryable();
                if (!String.IsNullOrEmpty(Name))
                    certificates = certificates.Where(i => (i.User!.LastName!.Contains(Name) || i.User!.FirstName!.Contains(Name)));
                if (UserId != null)
                    certificates = certificates.Where(i => (i.UserId == UserId));
                certificates = Sort switch
                {
                    CertificatesOrder.Id => certificates.OrderBy(c => c.CertificateId),
                    CertificatesOrder.IdDesc => certificates.OrderByDescending(c => c.CertificateId),
                    CertificatesOrder.Name => certificates.OrderBy(c => c.User!.LastName).ThenBy(c => c.User!.FirstName),
                    CertificatesOrder.NameDesc => certificates.OrderByDescending(c => c.User!.LastName).ThenByDescending(c => c.User!.FirstName),
                    CertificatesOrder.Date => certificates.OrderBy(c => c.Issued),
                    CertificatesOrder.DateDesc => certificates.OrderByDescending(c => c.Issued),
                    _ => certificates
                };
                Certificates = await PaginatedList<CertificateListVM>.CreateAsync(
                    certificates.Select(x => new CertificateListVM
                    {
                        CertificateId = x.CertificateId,
                        Title = x.Title,
                        Text = x.Text,
                        Description = x.Description,
                        Issued = x.Issued,
                        User = x.User,
                        CreatedBy = x.CreatedBy,
                        Enrollment = x.Enrollment,
                    }),
                    PageIndex ?? 1, PageSize ?? 100);
            }
        }
    }

    public enum CertificatesOrder
    {
        Id,
        IdDesc,
        Name,
        NameDesc,
        Date,
        DateDesc
    }
}
