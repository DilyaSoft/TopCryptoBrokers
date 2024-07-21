using Newtonsoft.Json;
using System;
using TopCrypto.DataLayer.Services.CoinIds.Models;

namespace TopCrypto.DataLayer.Services.CoinInfo.Models
{
    public class CoinInfoDTO : CoinIdsDTO, ICloneable
    {
        [JsonProperty("price_usd")]
        public double? PriceUsd { get; set; }
        [JsonProperty("price_btc")]
        public double? PriceBtc { get; set; }
        [JsonProperty("percent_change_24h")]
        public double? PercentChange24h { get; set; }
        [JsonProperty("market_cap_usd")]
        public double? MarketCapUsd { get; set; }
        [JsonProperty("last_updated")]
        public long? LastUpdated { get; set; }

        public override object Clone()
        {
            return new CoinInfoDTO()
            {
                Id = this.Id,
                Name = this.Name,
                PriceUsd = this.PriceUsd,
                PriceBtc = this.PriceBtc,
                PercentChange24h = this.PercentChange24h,
                MarketCapUsd = this.MarketCapUsd,
                LastUpdated = this.LastUpdated
            };
        }
    }
}
