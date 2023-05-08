using MongoDB.Driver;
using projOnTheFly.Company.Config;
using projOnTheFly.Models;

namespace projOnTheFly.Company.Services
{
    public class CompanyService
    {
        private readonly IMongoCollection<Models.Company> _company;

        public CompanyService() { }
        public CompanyService(IProjOnTheFlyCompanySettings settings)
        {
            var company = new MongoClient(settings.ConnectionString);
            var database = company.GetDatabase(settings.DatabaseName);
            _company = database.GetCollection<Models.Company>(settings.CompanyCollectionName);         

        }

        public List<Models.Company> Get() => _company.Find(a => true).ToList();
        public Models.Company Get(string cnpj) => _company.Find(c => c.Cnpj == cnpj).FirstOrDefault();
        public Models.Company Create(Models.Company company)
        {
            _company.InsertOne(company);
            return company;
        }

        public void Update(string cnpj, Models.Company company) => _company.ReplaceOne(a => a.Cnpj == cnpj, company);

        public void Delete(string cnpj) => _company.DeleteOne(a => a.Cnpj == cnpj);

        public void Delete(Models.Company company) => _company.DeleteOne(a => a.Cnpj == company.Cnpj);
    }
}
