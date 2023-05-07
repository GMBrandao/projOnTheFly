namespace projOnTheFly.Aircrafts.Config
{
    public interface IprojOnTheFlyAircraftSettings
    {
        string AircraftsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
