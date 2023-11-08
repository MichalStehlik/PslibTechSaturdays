using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.Areas.Admin.Pages.Tags
{
    public class CreateModel : PageModel
    {
        private readonly PslibTechSaturdays.Data.ApplicationDbContext _context;

        public CreateModel(PslibTechSaturdays.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Tag = new Tag { ForegroundColor = "#ffffff", BackgroundColor = "#555555" };
            return Page();
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Tags == null || Tag == null)
            {
                return Page();
            }

            _context.Tags.Add(Tag);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
