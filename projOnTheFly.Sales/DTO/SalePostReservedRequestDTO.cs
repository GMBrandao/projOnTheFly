using System.ComponentModel.DataAnnotations;
using projOnTheFly.Models;

namespace projOnTheFly.Sales.DTO
{
    public class SalePostReservedRequestDTO
    {
        [Required]
        [StringLength(3)]
        public string Iata { get; set; }

        [Required]
        [MaxLength(6)]
        public string Rab { get; set; }

        [Required]
        public DateTime Schedule { get; set; }

        [Required]
        public List<string> Passengers { get; set; }

        [Required]
        public bool Reserved { get; set; }
    }
}
