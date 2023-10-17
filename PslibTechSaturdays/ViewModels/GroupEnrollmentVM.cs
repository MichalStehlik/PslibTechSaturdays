using PslibTechSaturdays.Models;

namespace PslibTechSaturdays.ViewModels
{
    public class GroupEnrollmentVM
    {
        public int GroupId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string ShortenedDescription { get; set; } = string.Empty;
        public int EnrollmentsCount { get; set; } = 0;
        public int ParticipantsCount { get; set; } = 0;
        public int Capacity { get; set; } = 0;
        public ICollection<Tag> Tags { get; set; }
        public int UsersEnrollments { get; set; } = 0;
        public DateTime? OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public SchoolGrade MinGrade { get; set; }
        public bool EnrollmentsCountVisible { get; set; }
    }
}
