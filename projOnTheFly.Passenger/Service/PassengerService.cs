namespace projOnTheFly.Passenger.Service
{
    public class PassengerService
    {
        public string CPF { get; set; }
        public string Name { get; set; }
        public char Gender { get; set; }
        public string Phone { get; set; }
        public DateOnly DateBirth { get; set; }
        public DateTime DtRegister { get; set; }
        public bool Status { get; set; }
        public Address Address { get; set; } 

    }
}
