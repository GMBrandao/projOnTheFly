using MongoDB.Bson.Serialization.Attributes;

namespace projOnTheFly.Models.Entities
{
    public class Sale
    {
        [BsonId]
        public string Id { get; set; }
        public List<string> Passengers { get; set; }
        public Flight Flights { get; set; }
        public bool Reserved { get; set; }
        public bool Sold { get; set; }
        private string _id;

        public Sale(List<string> passagenrs, Flight flight)
        {
            var temp = Guid.NewGuid();
            _id = temp.ToString().Substring(0, 8);
            //GRU|PT-AAC|080520232149
            Id = $"{flight.Airport.iata}|{flight.Aircraft.Rab}|{flight.Schedule.Date.ToString("ddMMyyyyHHmm")}-{_id}";
            Passengers = passagenrs;
            Flights = flight;
        }

        public Sale()
        {
            
            
        }

    }
}
