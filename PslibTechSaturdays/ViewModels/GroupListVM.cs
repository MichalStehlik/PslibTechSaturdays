using PslibTechSaturdays.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PslibTechSaturdays.ViewModels
{
    public class GroupListVM
    {
        public int GroupId { get; set; }
        [Required]
        [DisplayName("Název")]
        public string Name { get; set; } = String.Empty;
        [Required]
        [DisplayName("Akce")]
        [ForeignKey("ActionId")]
        public Models.Action? Action { get; set; }
        public int ActionId { get; set; }
        [DisplayName("Kapacita")]
        public int Capacity { get; set; }
        [DisplayName("Lektoři")]
        public int LectorsCount { get; set; }
        [DisplayName("Přihlášky")]
        public int EnrollmentsCount { get; set; }
        [DisplayName("Zapsaní")]
        public int ParticipantsCount { get; set; }
        [DisplayName("Přítomní")]
        public int ParticipantsPresentCount { get; set; }
        [DisplayName("Stav")]
        public GroupState State { get; set; }
    }
}
