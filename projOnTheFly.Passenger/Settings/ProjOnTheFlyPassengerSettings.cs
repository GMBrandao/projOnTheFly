
namespace projOnTheFly.Passenger.Settings;

public class ProjOnTheFlyPassengerSettings : IProjOnTheFlyPassengerSettings
{
    public string PassengerCollectionName { get; set; }
    public string AddressCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
