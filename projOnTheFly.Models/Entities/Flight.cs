


using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using projOnTheFly.Models.DTO;

namespace projOnTheFly.Models.Entities
{
    public class Flight
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public AirportDTO Airport { get; set; }
        public Aircraft Aircraft { get; set; }
        public int Sale { get; set; }
        public bool Status { get; set; }
        public DateTime Schedule { get; set; }
    }
}
