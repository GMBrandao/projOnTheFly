namespace projOnTheFly.Passenger.Settings
{
    public interface IProjOnTheFlyPassengerSettings
    {
        public string PassengerCollectionName { get; set; }
        public string AddressCollectionName { get; set; }
        public string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
