using System.ComponentModel.DataAnnotations;

namespace projOnTheFly.Models.DTO
{
    public class FlightDecrementCheckDTO
    {

        public FlightDecrementCheckDTO(string iata, string rab, DateTime schedule, int number)
        {
            Iata = iata;
            Rab = rab;
            Schedule = schedule;
            Number = number;
        }

        [Required]
        [StringLength(3)]
        public string Iata { get; set; }

        [Required]
        [MaxLength(6)]
        public string Rab { get; set; }

        [Required]
       
        public DateTime Schedule { get; set; }
       
        [Required]
        [Range(1, int.MaxValue)]
        public int Number { get; set; }
    }
}
