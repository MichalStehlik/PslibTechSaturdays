using System.ComponentModel.DataAnnotations;

namespace PslibTechSaturdays.Models
{
    public enum GroupState
    {
        [Display(Name = "Založená")]
        Fresh = 0,
        [Display(Name = "Neotevřená")]
        Waiting = 1,
        [Display(Name = "Otevřená")]
        Opened = 2,
        [Display(Name = "Uzavřená")]
        Closed = 3,
        [Display(Name = "Jiný")]
        Errorneouns = 4,
    }
}
