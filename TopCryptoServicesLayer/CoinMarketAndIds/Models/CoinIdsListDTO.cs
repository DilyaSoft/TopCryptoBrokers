using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TopCrypto.DataLayer.Services.CoinIds.Models;

namespace TopCrypto.ServicesLayer.CoinMarketAndIds.Models
{
    public class CoinIdsListDTO : CoinIdsDTO
    {
        public CoinIdsListDTO() { }

        public CoinIdsListDTO(CoinIdsDTO dto)
        {
            this.Id = dto.Id;
            this.Name = dto.Name;
            this.Symbol = dto.Symbol;
        }

        [JsonProperty("markets")]
        public List<string> Markets { get; set; }

        public override object Clone()
        {
            return new CoinIdsListDTO()
            {
                Id = this.Id,
                Name = this.Name,
                Symbol = this.Symbol,
                Markets = this.Markets.ToList()
            };
        }
    }
}
