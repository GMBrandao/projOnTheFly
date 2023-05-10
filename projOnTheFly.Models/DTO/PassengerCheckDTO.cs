using System.ComponentModel.DataAnnotations;

namespace projOnTheFly.Models.DTO
{
    public class PassengerCheckDTO
    {
        [Required]
        public List<string> CpfList { get; set; }

    }
}
