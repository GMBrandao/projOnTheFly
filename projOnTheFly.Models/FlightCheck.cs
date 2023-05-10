namespace projOnTheFly.Passenger.DTO
{
    public class FlightCheck
    {

        public FlightCheck(string iata, string rab, DateTime schedule)
        {
            Iata = iata;
            Rab = rab;
            Schedule = schedule;
        }

        public string Iata { get;  set; }
        public string Rab { get;  set; }
        public DateTime Schedule { get; set; }
    }
}
