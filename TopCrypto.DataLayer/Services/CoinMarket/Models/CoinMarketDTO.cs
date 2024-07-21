using Newtonsoft.Json;

namespace TopCrypto.DataLayer.Services.CoinMarket.Models
{
    public class CoinMarketDTO
    {
        [JsonProperty("asset_id_base")]
        public string AssetIdBase { get; set; }

        [JsonProperty("symbol_id")]
        public string SymbolId { get; set; }

        public override string ToString()
        {
            return string.Format("{0}  {1}", AssetIdBase, SymbolId);
        }
    }
}
