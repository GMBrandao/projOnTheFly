using MongoDB.Driver;
using projOnTheFly.Aircrafts.Config;
using projOnTheFly.Models.Entities;

namespace projOnTheFly.Aircrafts.Services
{
    public class AircraftsService
    {
        private readonly IMongoCollection<Aircraft> _aircraft;
        
        public AircraftsService(IprojOnTheFlyAircraftSettings aircraftSettings)
        {
            var client = new MongoClient(aircraftSettings.ConnectionString);
            var database = client.GetDatabase(aircraftSettings.DatabaseName);
            _aircraft = database.GetCollection<Aircraft>(aircraftSettings.AircraftsCollectionName);
        }

        public Task<List<Aircraft>> GetAll() => _aircraft.Find<Aircraft>(a => true).ToListAsync();

        public Task<Aircraft> Get(string rab) => _aircraft.Find<Aircraft>(a => a.Rab == rab).FirstOrDefaultAsync();
        
        public async Task<Aircraft> Create(Aircraft aircraft)
        {
            await _aircraft.InsertOneAsync(aircraft);
            return aircraft;
        }

        public async Task<Aircraft> Update(string rab, Aircraft aircraft)
        {
            await _aircraft.ReplaceOneAsync(a => a.Rab == rab, aircraft);
            return aircraft;
        }

        public Task Delete(string rab) => _aircraft.DeleteOneAsync(a => a.Rab == rab);
    }
}
