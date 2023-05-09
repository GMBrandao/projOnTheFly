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
        
        public async Task<List<Flight>> Get() => await _collection.Find(c => c.Status == true).ToListAsync();

        public Task<Flight> Get(string iata, string rab, DateTime schedule) => _collection.Find(f => f.Airport.Iata == iata && f.Aircraft.Rab ==rab && f.Schedule == schedule).FirstOrDefaultAsync();


        public async Task<Flight> Create(Flight flight)
        {
            await _collection.InsertOneAsync(flight);
            return flight;
        }       
        
        public async void Update(string iata, string rab, DateTime schedule, Flight flight) => await  _collection.ReplaceOneAsync(f => f.Airport.Iata == iata && f.Aircraft.Rab == rab && f.Schedule == schedule, flight);

         public async void Delete(string iata, string rab, DateTime schedule, Flight flight) => await _collection.DeleteOneAsync(f => f.Airport.Iata == iata && f.Aircraft.Rab == rab && f.Schedule == schedule);
    }
}
