using MongoDB.Bson.Serialization.Attributes;

namespace projOnTheFly.Models
{
    public class Sale
    {
        [BsonId]
        public string Id { get; set; }
        public List<string>  Passengers { get; set; }
        public Flight Flights { get; set; }
        public bool Reserved { get; set; }
        public bool Sold { get; set; }


        public Sale(List<string> passagenrs, Flight flight, bool sold)
        {
            //GRU|PT-AAC|080520232149
            Id = $"{flight.Aircraft.Iata}|{flight.Aircraft.Rab}|{flight.Schedule.Date.ToString("ddMMyyyyHHmm")}"; 
            Passengers = passagenrs ;
            Flights = flight ;
            Sold = sold ;
            Reserved = !sold;
        }

    }
}
