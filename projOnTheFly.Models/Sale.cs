using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projOnTheFly.Models
{
    public class Sale
    {
        public List<Passenger>  Passenger { get; set; }
        public Flight Flight { get; set; }
        public bool Reserved { get; set; }
        public bool Sold { get; set; }
    }
}
