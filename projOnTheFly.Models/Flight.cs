using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projOnTheFly.Models
{
    public class Flight
    {
        public Airport Airport { get; set; }
        public Aircraft Aircraft { get; set; }
        public int Sale { get; set; }
        public bool Status { get; set; }
        public DateTime Schedule { get; set; }
    }
}
