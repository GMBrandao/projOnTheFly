using projOnTheFly.Models;

namespace projOnTheFly.Sales.DTO
{
    public class SalePostReservedRequest
    {
        public string Id { get; set; }
        public List<string> Passengers { get; set; }
        public Flight Flight { get; set; }
        public bool Reserved { get; set; }
    }
}
