using System.Reflection;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<Flight> Create(Flight flight)
        {
            await _collection.InsertOneAsync(flight);
            return flight;
        }
        public async Task<Flight> Update(Flight flight)
        {
            var filter = Builders<Flight>.Filter;

            var filterIata = filter.Eq(x => x.Airport.iata, flight.Airport.iata);
            var filterRab = filter.Eq(x => x.Aircraft.Rab, flight.Aircraft.Rab);
            var filterSchedule =  filter.Eq(x => x.Schedule, flight.Schedule);

            var filterAnd = filter.And(filterIata, filterRab, filterSchedule);

            await _collection.ReplaceOneAsync(filterAnd, flight);

            return flight;
        }

        public async Task<Flight> CheckFlight(string iata, string rab, DateTime schedule)
        {
            var filter = Builders<Flight>.Filter;

            var filterIata = filter.Eq(x => x.Airport.iata, iata);
            var filterRab = filter.Eq(x => x.Aircraft.Rab, rab);
            var filterSchedule = filter.Eq(x => x.Schedule, schedule);

            var filterAnd = filter.And(filterIata, filterRab, filterSchedule);

            return await _collection.Find(filterAnd).FirstOrDefaultAsync();
        }      
             
        public ActionResult<Flight> Delete(string iata, string rab, DateTime schedule)
        {
            var filter = Builders<Flight>.Filter;

            var filterIata = filter.Eq(x => x.Airport.iata, iata);
            var filterRab = filter.Eq(x => x.Aircraft.Rab, rab);
            var filterSchedule = filter.Eq(x => x.Schedule, schedule);

            var filterAnd = filter.And(filterIata, filterRab, filterSchedule);

            return  _collection.FindOneAndDelete(filterAnd);
        }
    }
}
