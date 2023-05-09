using MongoDB.Bson.Serialization.Attributes;

namespace projOnTheFly.Models
{
    public class Company
    {
        [BsonId]
        public string Cnpj { get; set; }
        public string Name { get; set; }
        public string NameOpt { get; set; }
        public DateTime DtOpen { get; set; } 
        public bool Status { get; set; }
        public Address Address { get; set; }
    }
}
