using System;
using System.Collections.Generic;
using TopCrypto.DataLayer.Services.FiatCurrency.Models;
using TopCrypto.ServicesLayer.FiatCurrency.Models;
using TopCrypto.ServicesLayer.Interfaces;
using TopCrypto.ServicesLayer.SocketServices.WebSockets;

namespace TopCrypto.ServicesLayer.FiatCurrency
{
    public class FiatCurrencyComparisonService : AbstractComparisonService<FiatCurrencyComparisonDTO, FiatCurrencyDTO>
    {
        public override FiatCurrencyComparisonDTO ConvertToComparisonDTO(FiatCurrencyDTO marketPriceDTO, SocketComparisonEnum comparison)
        {
            if (marketPriceDTO == null) return null;

            return new FiatCurrencyComparisonDTO()
            {
                Code = comparison,
                Name = marketPriceDTO.Name,
                Value = marketPriceDTO.Value,
            };
        }

        public override bool EqualityByData(FiatCurrencyDTO item1, FiatCurrencyDTO item2)
        {
            return item1.Value == item2.Value;
        }

        public override bool EqualityById(FiatCurrencyDTO item1, FiatCurrencyDTO item2)
        {
            return string.Equals(item1.Name, item2.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public override IList<FiatCurrencyDTO> OrderList(IList<FiatCurrencyDTO> lastResult)
        {
            return lastResult;
        }
    }
}
