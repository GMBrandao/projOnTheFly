using MongoDB.Driver;
using projOnTheFly.Passenger.Controllers;

namespace projOnTheFly.Passenger.Service
{
    public class PassengerService
    {
        private readonly IMongoCollection<Passenger> _passenger;
        public PassengerService(IProjOnTheFlyPassengerSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _passenger = database.GetCollection<Passenger>(settings.PassengerCollectionName);
        }
        public List<Passenger> Get() => _passenger.Find(c => true).ToList();
        public Passenger Get(string id) => _passenger.Find(c => c.Id == id).FirstOrDefault();
        public Passenger Create(Passenger passenger)
        {
            _passenger.InsertOne(passenger);
            return passenger;
        }
        public void Update(string id, Passenger passenger) => _passenger.ReplaceOne(c => c.Id == id, passenger);
        public void Delete(string id) => _passenger.DeleteOne(c => c.Id == id);
    }
}
