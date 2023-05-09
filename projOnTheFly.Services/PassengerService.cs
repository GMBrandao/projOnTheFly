using System.Net.Http.Json;
using Newtonsoft.Json;
using projOnTheFly.Models;

namespace projOnTheFly.Services
{
    public class PassengerService
    {
        static readonly HttpClient address = new HttpClient();
        public static async Task<List<Passenger>> CheckPassengers(List<string> cpfs)
        {
            try
            {
                HttpResponseMessage response = await address.PostAsJsonAsync($"https://localhost:7092/api/Passengers/check", cpfs);
                response.EnsureSuccessStatusCode();
                string ender = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Passenger>>(ender);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
