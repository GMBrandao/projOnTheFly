﻿namespace projOnTheFly.Flights.Config
{
    public interface IProjOnTheFlyFlightSettings
    {
        public string FlightCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
