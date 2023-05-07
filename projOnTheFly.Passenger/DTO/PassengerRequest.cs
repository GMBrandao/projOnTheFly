using projOnTheFly.Models;

namespace projOnTheFly.Passenger.DTO
{
    public class PassengerRequest
    {
        public string CPF { get; set; }
        public string Name { get; set; }
        public char Gender { get; set; }
        public string Phone { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime DtRegister { get; set; }
        public bool Status { get; set; }
        public Address Address { get; set; }
    }
}
