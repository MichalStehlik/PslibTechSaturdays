using Microsoft.EntityFrameworkCore;
using PslibTechSaturdays.Certificates.PageModels;
using PslibTechSaturdays.Data;
using PslibTechSaturdays.Models;

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

        public async Task<string> GetHtmlAsync(Guid id)
        {
            Certificate? cert = await _context.Certificates.FirstOrDefaultAsync(x => x.CertificateId == id);
            string certificateHtml = await _renderer.RenderViewToStringAsync("/Certificates/Pages/Simple.cshtml",
                new SimpleCertificateVM
                {
                    Title = cert!.Title,
                });
            return certificateHtml;
        }
    }
}
