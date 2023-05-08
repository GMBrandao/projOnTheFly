using MongoDB.Driver;
using projOnTheFly.Aircrafts.Config;

namespace projOnTheFly.Aircrafts.Services
{
    public class AircraftsService
    {
        private readonly IMongoCollection<Models.Aircraft> _aircraft;
        private readonly IMongoCollection<Models.Company> _company;
        
        public AircraftsService(IprojOnTheFlyAircraftSettings aircraftSettings)
        {
            var aircraft = new MongoClient(aircraftSettings.ConnectionString);
            var database = aircraft.GetDatabase(aircraftSettings.DatabaseName);
            _aircraft = database.GetCollection<Models.Aircraft>(aircraftSettings.AircraftsCollectionName);
        }

        public List<Models.Aircraft> GetAll() => _aircraft.Find<Models.Aircraft>(a => true).ToList();

        public Models.Aircraft Get(string rab) => _aircraft.Find<Models.Aircraft>(a => a.Rab == rab).FirstOrDefault();
        
        public Models.Aircraft Create(Models.Aircraft aircraft)
        {
            return aircraft;
        }
    }
}
