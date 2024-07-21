using Newtonsoft.Json;

namespace TopCrypto.DataLayer.Services.Broker.Models
{
    public class BrokerTopDTO
    {
        [JsonProperty(PropertyName = "origname")]
        public string BrokerName { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "imagePath")]
        public string UrlImg { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "mark")]
        public int? Mark { get; set; }
        [JsonProperty(PropertyName = "minDepositListing")]
        public string MinDepositListing { get; set; }
        [JsonProperty(PropertyName = "regulationListing")]
        public string RegulationListing { get; set; }
        [JsonProperty(PropertyName = "spreadsListing")]
        public string SpreadsListing { get; set; }
        [JsonProperty(PropertyName = "cryptos")]
        public CryptoDTO[] Cryptos { get; set; }
    }
}
