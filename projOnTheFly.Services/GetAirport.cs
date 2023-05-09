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

        static readonly HttpClient airportClient = new HttpClient();
        public static async Task<AirportDTO> GetAirportAsync(string iata)
        {
            try
            {   
                
                HttpResponseMessage response = await airportClient.GetAsync("https://localhost:44366/Airport/" + iata);
                response.EnsureSuccessStatusCode();
                string ender = await response.Content.ReadAsStringAsync();
                AirportDTO? airport = JsonConvert.DeserializeObject<AirportDTO?>(ender);

                airport = new()
                {
                    iata = airport.iata.ToUpper(),
                    City = airport.City,
                    Icao = airport.Icao.ToUpper(),
                    Country = "BR"
                };
                return airport;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }
    }
}
