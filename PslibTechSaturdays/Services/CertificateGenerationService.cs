using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Prints.PageModels;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

using System.Text;

namespace PslibTechSaturdays.Services
{
    public class CertificateGenerationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EnrollmentsService> _logger;
        private readonly RazorViewToStringRenderer _renderer;

        public CertificateGenerationService(ApplicationDbContext context, ILogger<EnrollmentsService> logger, RazorViewToStringRenderer renderer)
        {
            _context = context;
            _logger = logger;
            _renderer = renderer;
        }

        public async Task<string> GetHtmlAsync(Certificate cert)
        {
            string certificateHtml = await _renderer.RenderViewToStringAsync("/Prints/Pages/Simple.cshtml",
                    new SimpleCertificateVM
                    {
                        Title = cert!.Title,
                        Description = cert!.Description,
                        Text = cert!.Text,
                        IssueDate = cert.Issued,
                        User = cert.User,
                        CertificateId = cert.CertificateId
                    });
            return certificateHtml;
        }
    }
}
