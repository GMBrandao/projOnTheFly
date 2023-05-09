using System.ComponentModel.DataAnnotations;

namespace projOnTheFly.Aircrafts.DTO
{
    public class AircraftPost
    {
        [Required]
        [StringLength(6)]
        public string Rab { get; set; }

        [Required]
        public int Capacity { get; set; }
        
        [Required]
        public string cnpjCompany { get; set; }
    }
}
