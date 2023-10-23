using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Certificates
{
    public class IndexModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public IndexModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Certificate> Certificate { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Certificates != null)
            {
                Certificate = await _context.Certificates
                .Include(c => c.CreatedBy)
                .Include(c => c.User).ToListAsync();
            }
        }
    }
}
