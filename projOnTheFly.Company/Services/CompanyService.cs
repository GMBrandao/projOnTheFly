using MongoDB.Driver;
using projOnTheFly.Company.Config;
using projOnTheFly.Models;

namespace projOnTheFly.Company.Services
{
    public class CompanyService
    {
        private readonly IMongoCollection<Models.Company> _company;

        public CompanyService(IProjOnTheFlyCompanySettings settings)
        {
            var company = new MongoClient(settings.ConnectionString);
            var database = company.GetDatabase(settings.DatabaseName);
            _company = database.GetCollection<Models.Company>(settings.CompanyCollectionName);         

        }

        public async Task<List<Models.Company>> Get() => await _company.Find(a => true && a.Status == true).ToListAsync();
        public async Task<Models.Company> Get(string cnpj) => await _company.Find(c => c.Cnpj == cnpj ).FirstOrDefaultAsync();
        public Models.Company GetCompany(string cnpj) => _company.Find(c => c.Cnpj == cnpj && c.Status == true).FirstOrDefault();
        public async Task<Models.Company> Create(Models.Company company)
        {
            await _company.InsertOneAsync(company);
            return company;
        }

        public async void Update(string cnpj, Models.Company company) => await _company.ReplaceOneAsync(a => a.Cnpj == cnpj, company);

        public async void Delete(string cnpj) => await _company.DeleteOneAsync(a => a.Cnpj == cnpj);

       
    }
}
