using MongoDB.Bson.Serialization.Attributes;

namespace projOnTheFly.Models
{
    public class Passenger
    {
        [BsonId]
        public string CPF { get; set; }
        public string Name { get; set; }
        public char Gender { get; set; }
        public string Phone { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime DtRegister { get; set; }
        public bool Status { get; set; }
        public Address Address { get; set; }
     
        public string RemovePhoneMask(string phone)
        {
            return phone.Replace("(", "").Replace(")", "").Replace("-", "").Trim();
        }

        public bool IsUnderage()
        {
            bool underage = false;
            
            var passagerAge = DtRegister.Year - DateBirth.Year;

            if (passagerAge < 18)
                underage = true;
            else underage = false;

            return underage;
        }  
    }
}
