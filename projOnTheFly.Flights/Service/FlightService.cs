using System.Reflection;
using MongoDB.Driver;
using projOnTheFly.Flights.Config;
using projOnTheFly.Models;

namespace projOnTheFly.Flights.Service
{
    public class FlightService
    {
        private readonly IMongoCollection<Flight> _collection;
        public FlightService(IProjOnTheFlyFlightSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Flight>(settings.FlightCollectionName);
        }
        //public async Task<List<Flight>> Get() => await _collection.Find(c => c.Status == true).ToListAsync();
        //public Task<Flight> Get(string cpf) => _collection.Find(c => true).FirstOrDefaultAsync();
        public async Task<Flight> Create(Flight flight)
        {
            //await _collection.InsertOneAsync(flight);
            return flight;
        }
        public async Task<Flight> Update(Flight flight)
        {
           // await _collection.ReplaceOneAsync(c => true, flight);
            return flight;
        }
        //public Task Delete(string cpf) => _collection.DeleteOneAsync(c => c.CPF == cpf);
    }
}
