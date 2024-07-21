using Newtonsoft.Json;

namespace TopCrypto.DataLayer.Services.CoinInfo.Models
{
    public class CryptoCurencyPriceDTO
    {
        [JsonProperty("cryptoId")]
        public int CryptoId { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }
    }
}
