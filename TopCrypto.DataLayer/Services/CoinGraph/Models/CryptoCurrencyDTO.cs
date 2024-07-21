using Newtonsoft.Json;

namespace TopCrypto.DataLayer.Services.CoinGraph.Models
{
    public class CryptoCurrencyDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("symbol")]
        public string Abb { get; set; }
        [JsonProperty("selectedMarket")]
        public string MarketId { get; set; }
    }
}
