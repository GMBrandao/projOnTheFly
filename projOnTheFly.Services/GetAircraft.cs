using Newtonsoft.Json;
using projOnTheFly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projOnTheFly.Services
{
    public class GetAircraft
    {
        static readonly HttpClient address = new HttpClient();
        public static async Task<Aircraft> GetAircraftAsync(string rab)
        {
            try
            {
                HttpResponseMessage response = await address.GetAsync("https://localhost:7258/api/Aircrafts/" + rab);
                response.EnsureSuccessStatusCode();
                string ender = await response.Content.ReadAsStringAsync();
                Aircraft? aircraft = JsonConvert.DeserializeObject<Aircraft>(ender);
                return aircraft;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }
    }
}
