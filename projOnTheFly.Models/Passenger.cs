using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace projOnTheFly.Models
{
    public class Passenger
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CPF { get; set; }
        public string Name { get; set; }
        public char Gender { get; set; }
        public string Phone { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime DtRegister { get; set; }
        public bool Status { get; set; }
        public Address Address { get; set; }
     
        public bool IsUnderage()
        {
            bool underage = false;
            int age = 0;
            var passagerAge = DtRegister.Year - DateBirth.Year;
            if (passagerAge < 18)
                 underage = true;
            else underage = false;

            return underage;
        }  
    }
}
