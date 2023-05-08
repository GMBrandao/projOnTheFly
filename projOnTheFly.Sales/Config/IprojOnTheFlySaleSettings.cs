namespace projOnTheFly.Sales.Config
{
    public interface IProjOnTheFlySaleSettings
    {
        public string PassengerCollectionName { get; set; }
        public string SaleCollectionName { get; set; }
        public string FlightCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
