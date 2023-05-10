using MongoDB.Driver;
using projOnTheFly.Company.Config;

namespace projOnTheFly.Company.Services
{
    public class CompanyService
    {
        private readonly IMongoCollection<Models.Entities.Company> _company;
        private readonly IMongoCollection<Models.Entities.Company> _deletedCompany;

        public CompanyService(IProjOnTheFlyCompanySettings settings)
        {
            var company = new MongoClient(settings.ConnectionString);
            var database = company.GetDatabase(settings.DatabaseName);
            _company = database.GetCollection<Models.Entities.Company>(settings.CompanyCollectionName);
            _deletedCompany = database.GetCollection<Models.Entities.Company>(settings.DeletedCollectionName);


        }

        public async Task<List<Models.Entities.Company>> Get() => await _company.Find(a => true && a.Status == true).ToListAsync();
        public  Models.Entities.Company Get(string cnpj) =>  _company.Find(c => c.Cnpj == cnpj && c.Status == true).FirstOrDefault(); 
        public Models.Entities.Company GetCompany(string cnpj) => _company.Find(c => c.Cnpj == cnpj && c.Status == true).FirstOrDefault();
        public async Task<Models.Entities.Company> Create(Models.Entities.Company company)
        {
            await _company.InsertOneAsync(company);
            return company;
        }

        public async void AddInDeletedCollection(Models.Entities.Company company)
        {
            await _deletedCompany.InsertOneAsync(company);
            
        }

        public async void Update(string cnpj, Models.Entities.Company company) => await _company.ReplaceOneAsync(a => a.Cnpj == cnpj, company);

        public async void Delete(string cnpj)
        {
            var company = _company.Find(c => c.Cnpj == cnpj && c.Status == true).FirstOrDefaultAsync();
            
            await _company.DeleteOneAsync(a => a.Cnpj == cnpj);
        }

        
    }
}
