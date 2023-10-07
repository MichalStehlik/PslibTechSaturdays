using System.ComponentModel.DataAnnotations;

namespace PslibTechSaturdays.Models
{
    public enum Presence
    {
        [Display(Name = "Neznámo")]
        Unknown = 0,
        [Display(Name = "Přítomen")]
        Present = 1,
        [Display(Name = "Nepřítomen")]
        Absent = 2
    }
}
