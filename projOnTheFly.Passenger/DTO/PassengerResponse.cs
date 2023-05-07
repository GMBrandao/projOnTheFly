using projOnTheFly.Models;

namespace projOnTheFly.Passenger.DTO
{
    public class PassengerResponse
    {
        public string Name { get; set; }
        public DateTime DtRegister { get; set; }
        public bool Status { get; set; }
    }
}
