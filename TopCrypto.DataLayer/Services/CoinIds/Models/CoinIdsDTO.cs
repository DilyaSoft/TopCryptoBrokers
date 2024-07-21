using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TopCrypto.DataLayer.Services.CoinIds.Models
{
    public class CoinIdsDTO : ICloneable, IEquatable<CoinIdsDTO>
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        public virtual object Clone()
        {
            return new CoinIdsDTO()
            {
                Id = this.Id,
                Name = this.Name,
                Symbol = this.Symbol
            };
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CoinIdsDTO);
        }

        public bool Equals(CoinIdsDTO other)
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
