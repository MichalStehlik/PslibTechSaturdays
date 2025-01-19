using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.ViewModels
{
    public class GroupWithLastEnrollmentVM
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ActionId { get; set; }
        public int Capacity { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public bool EnrollmentsCountVisible { get; set; }
        public List<ApplicationUser> Lectors { get; set; } = new List<ApplicationUser>();
        public int EnrollmentCount { get; set; }
        public int ActiveEnrollmentCount { get; set; } // Přidáno: Počet nezrušených přihlášek
        public DateTime? LastActiveEnrollment { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
