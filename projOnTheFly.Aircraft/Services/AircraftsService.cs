using MongoDB.Driver;
using projOnTheFly.Aircrafts.Config;
using projOnTheFly.Company.Services;
using projOnTheFly.Models;

namespace projOnTheFly.Aircrafts.Services
{
    public class AircraftsService
    {
        private readonly IMongoCollection<Aircraft> _aircraft;
        private readonly IMongoCollection<Models.Company> _company;
        
        public AircraftsService(IprojOnTheFlyAircraftSettings aircraftSettings)
        {
            var aircraft = new MongoClient(aircraftSettings.ConnectionString);
            var database = aircraft.GetDatabase(aircraftSettings.DatabaseName);
            _aircraft = database.GetCollection<Aircraft>(aircraftSettings.AircraftsCollectionName);
        }

        public List<Aircraft> GetAll() => _aircraft.Find<Aircraft>(a => true).ToList();

        public Aircraft Get(string rab) => _aircraft.Find<Aircraft>(a => a.Rab == rab).FirstOrDefault();
        
        public Aircraft Create(Aircraft aircraft)
        {
            

            return aircraft;
        }
    }
}
