using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using projOnTheFly.Models;
using projOnTheFly.Sales.Config;

namespace projOnTheFly.Sales.Service
{
    public class SaleService
    {
        private readonly IMongoCollection<Sale> _collection;
        public SaleService(IProjOnTheFlySaleSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Sale>(settings.SaleCollectionName);
        }
        public async Task<List<Sale>> GetAsync() => await _collection.Find(c => c.Sold == true).ToListAsync();

        public async Task<Sale> GetSaleByPassengerAsync(string cpf)
        {
           return  await _collection.Find(c=> c.Passengers.Contains(cpf)).FirstOrDefaultAsync();
        }
        public async Task<Sale> CreateAsync(Sale sale)
        {
            await _collection.InsertOneAsync(sale);
            return sale;
        }
        public async Task<Sale> UpdateAsync(Sale sale)
        {
            await _collection.ReplaceOneAsync(c => c.Id == sale.Id, sale);
            return sale;
        }
       
        public Task Delete(string cpf) => _collection.DeleteOneAsync(c => c.Passengers.Contains(cpf));

        public async Task<Sale> GetByFlightAsync(string iata, string rab, string schedule)
        {
            return await _collection.Find(c => c.Id == $"{iata}|{rab}|{schedule}").FirstOrDefaultAsync();
        }

        public async Task<Sale> GetByIdAsync(string id)
        {
            return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}
