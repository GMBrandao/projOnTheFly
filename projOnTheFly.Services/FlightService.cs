using Newtonsoft.Json;
using projOnTheFly.Models;

namespace projOnTheFly.Services
{
    public class FlightService
    {
        static readonly HttpClient address = new HttpClient();
        public static async Task<Flight?> GetFlightAsync(string iata, string rab, DateTime schedule)
        {
            try
            {
                HttpResponseMessage response = await address.GetAsync($"https://localhost:7068/api/Flights/{iata}/{rab}/{schedule}");
                response.EnsureSuccessStatusCode();
                string ender = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Flight>(ender);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
