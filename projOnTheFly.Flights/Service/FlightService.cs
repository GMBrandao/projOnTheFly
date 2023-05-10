using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using projOnTheFly.Flights.Config;
using projOnTheFly.Models.Entities;

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

        public  void  Update(string iata, string rab, DateTime schedule)
        {            

            var filter = Builders<Flight>.Filter.Where(f => f.Airport.iata == iata && f.Aircraft.Rab == rab && f.Schedule == schedule);
            var filt = Builders<Flight>.Update.Set(f => f.Status, false);
            _collection.UpdateOne(filter, filt);

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


        public  void Delete(string iata, string rab, DateTime schedule)
        {
               _collection.DeleteOne(f => f.Airport.iata == iata && f.Aircraft.Rab == rab && f.Schedule == schedule);
        }

        public async Task DecrementSale(string iata, string rab, DateTime schedule, int number)
        {
            var filter = Builders<Flight>.Filter;

            var filterIata = filter.Eq(x => x.Airport.iata, iata);
            var filterRab = filter.Eq(x => x.Aircraft.Rab, rab);
            var filterSchedule = filter.Eq(x => x.Schedule, schedule);

            var filterAnd = filter.And(filterIata, filterRab, filterSchedule);

            var filterUpdate = Builders<Flight>.Update.Inc(x => x.Aircraft.Capacity, (number * -1));

            await _collection.UpdateOneAsync(filterAnd, filterUpdate);
        }
    }
}
