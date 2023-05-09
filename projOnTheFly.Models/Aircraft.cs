using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace projOnTheFly.Models
{
    public class Aircraft
    {
        [Required]
        [StringLength(6)]
        [BsonId]
        public string Rab { get; set; }
      
        public int Capacity { get; set; }
        public DateTime DtRegistry { get; set; }
        public DateTime? DtLastFlight { get; set; }
        public Company Company { get; set; }
    }
}
