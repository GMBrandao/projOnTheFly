namespace projOnTheFly.Models
{
    public class Flight
    {
        public AirportDTO Airport { get; set; }
        public Aircraft Aircraft { get; set; }
        public int Sale { get; set; }
        public bool Status { get; set; }
        public DateTime Schedule { get; set; }
    }
}
