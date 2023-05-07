using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using projOnTheFly.Models;

namespace projOnTheFly.Services
{
    public class PostOfficeService
    {
        static readonly HttpClient address = new HttpClient();
        public static async Task<AddressDTO> GetAddress(string cep)
        {
            HttpResponseMessage response = await address.GetAsync("https://viacep.com.br/ws/" + cep + "/json/");
            response.EnsureSuccessStatusCode();
            string ender = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AddressDTO>(ender);
        }
    }
}
