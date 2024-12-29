using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PslibTechSaturdays.Data;
using System.Linq;

namespace PslibTechSaturdays.Pages
{
    public class CertificateModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CertificateModel> _logger;

        public string Message { get; private set; } = string.Empty;

        public CertificateModel(ApplicationDbContext context, ILogger<CertificateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void OnGet()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Message = "Zadejte ID osv�d�en�.";
                return;
            }

            if (!Guid.TryParse(Id, out Guid certificateId))
            {
                Message = "Neplatn� ID osv�d�en�.";
                return;
            }

            var certificate = _context.Certificates
                .Where(c => c.CertificateId == certificateId)
                .Select(c => new
                {
                    c.User.FirstName,
                    c.User.LastName,
                    c.Title,
                    c.Issued
                })
                .FirstOrDefault();

            if (certificate == null)
            {
                Message = "Osv�d�en� s t�mto ID neexistuje.";
            }
            else
            {
                string anonymizedName = $"{certificate.FirstName.First()}. {certificate.LastName.First()}.";
                Message = $"Osv�d�en� existuje. Jm�no: {anonymizedName}, Titulek: {certificate.Title}, Datum vyd�n�: {certificate.Issued:dd.MM.yyyy}.";
            }
        }
    }
}
