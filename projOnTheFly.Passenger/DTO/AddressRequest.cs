using System.ComponentModel.DataAnnotations;

namespace projOnTheFly.Passenger.DTO
{
    public class AddressRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Number { get; set; }

        public  string Complement { get; set; }

        [Required]
        [MaxLength(9)]
        public string? ZipCode { get; set; }
    }
}
