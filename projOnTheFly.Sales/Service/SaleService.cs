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
        public async Task<List<Sale>> Get() => await _collection.Find(c => c.Sold == true).ToListAsync();

        public async Task<Sale> GetSaleByPassenger(string cpf)
        {
           return  await _collection.Find(c=> c.Passengers.Contains(cpf)).FirstOrDefaultAsync();
        }
        public async Task<Sale> Create(Sale sale)
        {
            await _collection.InsertOneAsync(sale);
            return sale;
        }
        public async Task<Sale> Update(Sale sale)
        {
            await _collection.ReplaceOneAsync(c => c.Id == sale.Id, sale);
            return sale;
        }
       
        public Task Delete(string cpf) => _collection.DeleteOneAsync(c => c.Passengers.Contains(cpf));

        public async Task<ActionResult<Sale>> GetByFlight(string iata, string rab, string schedule)
        {
            return await _collection.Find(c => c.Id == $"{iata}|{rab}|{schedule}").FirstOrDefaultAsync();
        }

        public async Task<ActionResult<Sale>> GetById(string id)
        {
            return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}
