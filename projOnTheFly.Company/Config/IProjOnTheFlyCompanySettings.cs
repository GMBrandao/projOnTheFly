namespace projOnTheFly.Company.Config
{
    public interface IProjOnTheFlyCompanySettings
    {
        public string CompanyCollectionName { get; set; }
        public string DeletedCollectionName { get; set; }       
        public string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
