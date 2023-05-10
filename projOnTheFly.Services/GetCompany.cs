using Newtonsoft.Json;
using projOnTheFly.Models.Entities;

namespace projOnTheFly.Services
{
    public class GetCompany
    {
        static readonly HttpClient address = new HttpClient();
        public static async Task<Company> GetCompanyAsync(string cnpj)
        {
            try
            {
                string[] arr = cnpj.Split('/'); 
                HttpResponseMessage response = await address.GetAsync("https://localhost:7183/api/Company/" + arr[0] +"%2F" + arr[1]);
                response.EnsureSuccessStatusCode();
                string ender = await response.Content.ReadAsStringAsync();
                Company? company = JsonConvert.DeserializeObject<Company>(ender);
                return company;
            }
            catch(HttpRequestException ex)
            {
                return null;
            }
        }
    }
}
