using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace projOnTheFly.Models
{
    public class Company
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Cnpj { get; set; }
        public string Name { get; set; }
        public string NameOpt { get; set; }
        public DateTime DtOpen { get; set; } //? teste colokei Datetime ms pede DATE
        public bool Status { get; set; }
        public Address Address { get; set; }
    }
}
