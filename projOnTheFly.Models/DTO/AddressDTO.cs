using Newtonsoft.Json;

namespace projOnTheFly.Models.DTO
{
    public class AddressDTO
    {

        [JsonProperty("cep")]
        public string ZipCode { get; set; }

        [JsonProperty("bairro")]
        public string NeighborHood { get; set; }

        [JsonProperty("localidade")]
        public string City { get; set; }

        [JsonProperty("uf")]
        public string State { get; set; }

        [JsonProperty("logradouro")]
        public string Street { get; set; }

        [JsonProperty("complemento")]
        public string Complement { get; set; }
    }
}
