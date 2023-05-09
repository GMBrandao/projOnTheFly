using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
