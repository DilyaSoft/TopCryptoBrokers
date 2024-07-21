using Newtonsoft.Json;

namespace TopCrypto.ServicesLayer.CoinInfo.Models
{
    public class CoinIdNameDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
