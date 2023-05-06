namespace projOnTheFly.Company.Config
{
    public interface IProjOnTheFlyCompanySettings
    {
        public string CompanyCollectionName { get; set; }
        public string AddressCollectionName { get; set; }       
        public string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
