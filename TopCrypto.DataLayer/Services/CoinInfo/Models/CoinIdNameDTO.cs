using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TopCrypto.DataLayer.Services.CoinInfo.Models
{
    public class CoinIdNameDTO : IEquatable<CoinIdNameDTO>
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as CoinIdNameDTO);
        }

        public bool Equals(CoinIdNameDTO other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
        }
    }
}
