using projOnTheFly.Models;

namespace projOnTheFly.Sales.DTO
{
    public class SalePutRequest
    {

        public List<string> Passengers { get; set; }
        public bool Reserved { get; set; }
        public bool Sold { get; set; }
    }
}
