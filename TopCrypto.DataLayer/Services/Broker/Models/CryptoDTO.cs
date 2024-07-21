using Newtonsoft.Json;

namespace TopCrypto.DataLayer.Services.Broker.Models
{
    public class CryptoDTO
    {
        [JsonProperty(PropertyName = "cryptoId")]
        public string CryptoId { get; set; }
        [JsonProperty(PropertyName = "value")]
        public double Value { get; set; }
    }
}
