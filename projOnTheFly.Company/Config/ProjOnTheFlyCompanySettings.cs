namespace projOnTheFly.Company.Config
{
    public class ProjOnTheFlyCompanySettings : IProjOnTheFlyCompanySettings
    {
        public string CompanyCollectionName { get ; set ; }
        public string DeletedCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set ; }
    }
}
