using MongoDB.Driver;
using projOnTheFly.Passenger.Settings;

namespace projOnTheFly.Passenger.Service
{
    public class PassengerService
    {
        private readonly IMongoCollection<Models.Passenger> _collection;
        public PassengerService(IProjOnTheFlyPassengerSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Models.Passenger>(settings.PassengerCollectionName);
        }
        public async Task<List<Models.Passenger>> Get() => await _collection.Find(c => c.Status == true).ToListAsync();

        public Task<Models.Passenger> Get(string cpf) => _collection.Find(c => c.CPF == cpf && c.Status == true).FirstOrDefaultAsync();

        public async Task<Models.Passenger> Create(Models.Passenger passenger)
        {
            await _collection.InsertOneAsync(passenger);
            return passenger;
        }
        public async Task<Models.Passenger> Update(Models.Passenger passenger)
        {
            await _collection.ReplaceOneAsync(c => c.CPF == passenger.CPF, passenger);
            return passenger;
        }
        public Task Delete(string cpf) => _collection.DeleteOneAsync(c => c.CPF == cpf);
    }
}
