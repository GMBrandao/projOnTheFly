namespace projOnTheFly.Passenger.DTO
{
    public class FlightDecrementCheck
    {

        public FlightDecrementCheck(string iata, string rab, DateTime schedule, int number)
        {
            Iata = iata;
            Rab = rab;
            Schedule = schedule;
            Number = number;
        }

        public string Iata { get;  set; }
        public string Rab { get;  set; }
        public DateTime Schedule { get; set; }
        public int Number { get; set; }
    }
}
