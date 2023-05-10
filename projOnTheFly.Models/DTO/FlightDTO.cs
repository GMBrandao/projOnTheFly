using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projOnTheFly.Models.DTO
{
    public class FlightDTO
    {
        public string Iata { get; set; }
        public string Rab { get; set; }       
        public bool Status { get; set; }
        public DateTime Schedule { get; set; }
    }
}
