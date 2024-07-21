using Newtonsoft.Json;
using System;

namespace TopCrypto.DataLayer.Services.CoinGraph.Models
{
    public class CoinInfoGraphDTO
    {
        [JsonProperty("time_period_start")]
        public DateTime? CloseTime { get; set; }

        [JsonProperty("price_close")]
        public double? ClosePrice { get; set; }
    }
}
