using PslibTechSaturdays.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PslibTechSaturdays.ViewModels
{
    public class ActionListVM
    {
        public int ActionId { get; set; }
        [Required]
        [DisplayName("Název")]
        public string Name { get; set; } = String.Empty;
        [DisplayName("Školní rok")]
        public int Year { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        [DisplayName("Vytvořeno")]
        public DateTime Created { get; set; } = DateTime.Now;
        [DisplayName("Aktivní")]
        public bool Active { get; set; } = true;
        [DisplayName("Zveřejněná")]
        public bool Published { get; set; } = true;

        [DisplayName("Skupiny")]
        public int GroupsCount { get; set; } = 0;
    }
}
