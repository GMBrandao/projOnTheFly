﻿using System.Net.Http.Json;
using Newtonsoft.Json;
using projOnTheFly.Models;
using projOnTheFly.Models.DTO;

namespace projOnTheFly.Services
{
    public class PassengerService
    {
        static readonly HttpClient address = new HttpClient();
        public static async Task<List<PassengerCheckResponseDTO>> CheckPassengersAsync(PassengerCheckDTO passengerCheck)
        {
            try
            {
                HttpResponseMessage response = await address.PostAsJsonAsync($"https://localhost:7092/api/Passengers/check", passengerCheck);
                response.EnsureSuccessStatusCode();
                string ender = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<PassengerCheckResponseDTO>>(ender);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
