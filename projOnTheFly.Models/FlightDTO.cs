using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projOnTheFly.Models
{
    public class FlightDTO
    {
        public string Iata { get; set; }

        public string Rab { get; set; }
        public int Sales { get; set; }
        public bool Status { get; set; }
        public DateTime Schadule { get; set; }
    }
}
