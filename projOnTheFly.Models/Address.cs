using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace projOnTheFly.Models
{
    public class Address
    {
        
        public string? Street { get; set; }

        public int? Number { get; set; }

        public string? NeighborHood { get; set; }

        public string? ZipCode { get; set; }

        public string? Complement { get; set; }

        public string State { get; set; }

        public string? City { get; set; }
       
    }
}
