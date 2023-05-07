using MongoDB.Driver;
using projOnTheFly.Passenger.Settings;

namespace projOnTheFly.Passenger.Service
{
    public class PassengerService
    {
        private readonly IMongoCollection<Models.Passenger> _passenger;
        public PassengerService(IProjOnTheFlyPassengerSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _passenger = database.GetCollection<Models.Passenger>(settings.PassengerCollectionName);
        }
        public List<Models.Passenger> Get() => _passenger.Find(c => true).ToList();
        public Models.Passenger Get(string cpf) => _passenger.Find(c => c.CPF == cpf).FirstOrDefault();
        public Models.Passenger Create(Models.Passenger passenger)
        {
            _passenger.InsertOne(passenger);
            return passenger;
        }
        public void Update(string cpf, Models.Passenger passenger) => _passenger.ReplaceOne(c => c.CPF == cpf, passenger);
        public void Delete(string cpf) => _passenger.DeleteOne(c => c.CPF == cpf);
    }
}
