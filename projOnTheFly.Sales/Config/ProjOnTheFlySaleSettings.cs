namespace projOnTheFly.Sales.Config
{
    public class ProjOnTheFlySaleSettings : IProjOnTheFlySaleSettings
    {
        public string PassengerCollectionName { get; set; }
        public string FlightCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string SaleCollectionName { get; set; }
    }
}
