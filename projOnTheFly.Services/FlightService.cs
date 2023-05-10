using System.Net.Http.Json;
using Newtonsoft.Json;
using projOnTheFly.Models;
using projOnTheFly.Passenger.DTO;

namespace projOnTheFly.Services
{
    public class FlightService
    {
        static readonly HttpClient flight = new HttpClient();
        public static async Task<Flight?> CheckFlightAsync(string iata, string rab, DateTime schedule)
        {
            try
            {
                FlightCheck flightCheck = new FlightCheck(iata, rab, schedule);
                HttpResponseMessage response = await flight.PostAsJsonAsync($"https://localhost:7068/api/Flights/check", flightCheck);
                response.EnsureSuccessStatusCode();
                string ender = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Flight>(ender);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<bool> DecrementSaleAsync(string iata, string rab, DateTime schedule, int number)
        {
            try
            {
                FlightDecrementCheck flightDecrementCheck = new FlightDecrementCheck(iata, rab, schedule, number);
                HttpResponseMessage response = await flight.PostAsJsonAsync($"https://localhost:7068/api/Flights/decrement", flightDecrementCheck);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
