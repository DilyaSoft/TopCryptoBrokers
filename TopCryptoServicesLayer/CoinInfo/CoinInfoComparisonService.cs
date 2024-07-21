using System;
using TopCrypto.ServicesLayer.SocketServices.WebSockets;
using TopCrypto.ServicesLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TopCrypto.ServicesLayer.CoinInfo.Models;
using TopCrypto.DataLayer.Services.CoinInfo.Models;

namespace TopCrypto.ServicesLayer.CoinInfo
{
    public class CoinInfoComparisonService : AbstractComparisonService<CoinInfoComparisonDTO, CoinInfoDTO>
    {
        public override CoinInfoComparisonDTO ConvertToComparisonDTO(
            CoinInfoDTO marketPriceDTO,
            SocketComparisonEnum comparison)
        {
            if (marketPriceDTO == null) return null;

            return new CoinInfoComparisonDTO()
            {
                Code = comparison,
                Id = marketPriceDTO.Id,
                Name = marketPriceDTO.Name,
                MarketCapUsd = marketPriceDTO.MarketCapUsd,
                PercentChange24h = marketPriceDTO.PercentChange24h,
                PriceUsd = marketPriceDTO.PriceUsd,
                PriceBtc = marketPriceDTO.PriceBtc,
                LastUpdated = marketPriceDTO.LastUpdated
            };
        }

        public override bool EqualityByData(CoinInfoDTO item1, CoinInfoDTO item2)
        {
            return item1.PercentChange24h == item2.PercentChange24h &&
                            item1.MarketCapUsd == item2.MarketCapUsd &&
                            item1.PriceUsd == item2.PriceUsd &&
                            item1.PriceBtc == item2.PriceBtc;
        }

        public override bool EqualityById(CoinInfoDTO item1, CoinInfoDTO item2)
        {
            return string.Equals(item1.Id, item2.Id, StringComparison.CurrentCultureIgnoreCase);
        }

        public override IList<CoinInfoDTO> OrderList(IList<CoinInfoDTO> lastResult)
        {
            return lastResult.OrderByDescending((x) => x.PriceUsd).ToList();
        }
    }
}
