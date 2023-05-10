using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projOnTheFly.Models.DTO
{
    public class FlightDTO
    {
        [Required]
        [StringLength(3)]
        public string Iata { get; set; }

        [Required]
        [MaxLength(6)]
        public string Rab { get; set; }
        
        [Required]
        public int Sales { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public DateTime Schedule { get; set; }
    }
}
