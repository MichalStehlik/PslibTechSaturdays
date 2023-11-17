using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Emails.PageModels;
using PslibTechSaturdays.Helpers;
using PslibTechSaturdays.Models;
using PslibTechSaturdays.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PslibTechSaturdays.Areas.Admin.Pages.Users
{
    public class MailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RazorViewToStringRenderer _renderer;

        public MailModel( ApplicationDbContext context, ILogger<DetailsModel> logger, IEmailSender emailSender, RazorViewToStringRenderer renderer)
        {
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
            _renderer = renderer;
        }
        [BindProperty]
        public EmailInputModel? Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user is null)
            {
                return NotFound();
            }
            if (!user.EmailConfirmed)
            {
                return RedirectToPage("/Index");
            }
            Input = new EmailInputModel { UserId = id, To = user.Email };
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == Input!.UserId);
            if (user is null)
            {
                return NotFound();
            }
            string htmlBody = await _renderer.RenderViewToStringAsync("/Emails/Pages/Common.cshtml",
                    new CommonMessageVM
                    {
                        Body = Input.HtmlBody
                    });
            try
            {
                await _emailSender.SendEmailAsync(user.Email, Input.Subject, htmlBody);
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Success, "Zpráva byla odeslána.");
            }
            catch
            {
                TempData.AddMessage(Constants.Messages.COOKIE_ID, TempDataExtension.MessageType.Danger, "Pøi odesílání zprávy došlo k chybì.");
            }
            return RedirectToPage("Index");
        }
    }

    public class EmailInputModel
    {
        public Guid UserId { get; set; }
        public string? To { get; set; }
        [DisplayName("Pøedmìt")]
        [Required]
        public string? Subject { get; set; }
        [DisplayName("Zpráva")]
        public string? HtmlBody { get; set; }
    }
}
