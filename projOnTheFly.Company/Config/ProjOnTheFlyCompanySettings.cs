namespace projOnTheFly.Company.Config
{
    public class ProjOnTheFly : IProjOnTheFlyCompanySettings
    {
        public string CompanyCollectionName { get ; set ; }
        public string AddressCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set ; }
    }
}
