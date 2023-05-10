using System.ComponentModel.DataAnnotations;
using projOnTheFly.Models;

namespace projOnTheFly.Passenger.DTO
{
    public class PassengerPostRequestDTO
    {

        [Required]
        [StringLength(14)]
        public string CPF { get; set; }

        [Required]
        [MaxLength(10)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        public char Gender { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateBirth { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public AddressRequestDTO Address { get; set; }
    }
}
