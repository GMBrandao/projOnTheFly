using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using projOnTheFly.Models;

namespace projOnTheFly.Services
{
    public  class GetAirport
    {

        static readonly HttpClient address = new HttpClient();
        public static async Task<AirportDTO> GetAirportAsync(string iata)
        {
            try
            {   
                
                HttpResponseMessage response = await address.GetAsync("https://localhost:5001/Airport/" + iata);
                response.EnsureSuccessStatusCode();
                string ender = await response.Content.ReadAsStringAsync();
                Airport? airport = JsonConvert.DeserializeObject<Airport?>(ender);
                AirportDTO airportDTO = new()
                {
                    Iata = airport.iata,
                    State = airport.state,
                    City = airport.city,
                    Country = airport.country_id
                };
                return airportDTO;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }
    }
}
