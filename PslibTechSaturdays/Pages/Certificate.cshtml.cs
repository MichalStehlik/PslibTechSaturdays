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
                Message = "Zadejte ID osvìdèení.";
                return;
            }

            if (!Guid.TryParse(Id, out Guid certificateId))
            {
                Message = "Neplatné ID osvìdèení.";
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
                Message = "Osvìdèení s tímto ID neexistuje.";
            }
            else
            {
                string anonymizedName = $"{certificate.FirstName.First()}. {certificate.LastName.First()}.";
                Message = $"Osvìdèení existuje. Jméno: {anonymizedName}, Titulek: {certificate.Title}, Datum vydání: {certificate.Issued:dd.MM.yyyy}.";
            }
        }
    }
}
