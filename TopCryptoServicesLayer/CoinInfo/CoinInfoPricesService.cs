using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TopCrypto.DataLayer.Services.CustomCache;
using TopCrypto.DataLayer.Services.CoinInfo.Interfaces;
using TopCrypto.DataLayer.Services.CoinInfo.Models;
using TopCrypto.ServicesLayer.CoinInfo.Interfaces;

namespace TopCrypto.ServicesLayer.CoinInfo
{
    public class CoinInfoPricesService : ICryptoCurencyPriceSaver
    {
        private ICoinInfoPricesDataService _pricesDataService;
        private InternalCache _cache;
        private int CACHE_TIME_SECONDS => 60 * 20;
        protected static object locked = new object();

        public CoinInfoPricesService(ICoinInfoPricesDataService pricesDataService, InternalCache internalCache)
        {
            this._pricesDataService = pricesDataService;
            this._cache = internalCache;
        }

        public async Task SaveNewResult(IList<CoinInfoDTO> coinInfoList)
        {
            await _pricesDataService.SaveNewResult(coinInfoList);
        }

        public Dictionary<int, List<double>> GetWeekPriceAverage(out bool isFromCache
            , bool ignoreCacheExpiration = false)
        {
            lock (locked)
            {
                if (_cache.Get(InternalCacheKeys.WeekPriceAverage, null, ignoreCacheExpiration)
                    is Dictionary<int, List<double>> obj)
                {
                    isFromCache = true;
                    return obj;
                }
                var result = GetWeekPriceAverageWithoutCache();
                result.Wait();

                _cache.Add(InternalCacheKeys.WeekPriceAverage
                    , result.Result.ToDictionary(i => i.Key, i => new List<double>(i.Value))
                    , this.CACHE_TIME_SECONDS);

                isFromCache = false;
                return result.Result;
            }
        }

        private async Task<Dictionary<int, List<double>>> GetWeekPriceAverageWithoutCache()
        {
            var currencies = await _pricesDataService.GetAverageWeekValues();

            if (currencies == null || currencies.Count == 0) return new Dictionary<int, List<double>>();

            var resultDiction = new Dictionary<int, List<double>>();

            var lastId = currencies[0].CryptoId;
            var prices = new List<double>();
            foreach (var i in currencies)
            {
                if (i.CryptoId != lastId)
                {
                    resultDiction[lastId] = prices;
                    prices = new List<double>();
                    lastId = i.CryptoId;
                }
                prices.Add(i.Price);
            }
            resultDiction[lastId] = prices;

            return resultDiction;
        }


        public Dictionary<int, List<double>> GetWeekPriceAverage(IEnumerable<string> ids
            , out bool isFromCache
            , bool ignoreCacheExpiration)
        {
            var priceAverage = GetWeekPriceAverage(out isFromCache, ignoreCacheExpiration);

            return priceAverage.Where(dictionEntry =>
            {
                return ids.Any(item =>
                {
                    return item == dictionEntry.Key.ToString();
                });
            }).ToDictionary(i => i.Key, i => i.Value);
        }

        public Dictionary<int, List<double>> GetWeekPriceAverage(IEnumerable<string> ids)
        {
            return GetWeekPriceAverage(ids, out bool isFromCache, true);
        }

        public Dictionary<int, List<double>> GetWeekPriceAverage(IEnumerable<string> ids
            , Dictionary<int, List<double>> priceAverage)
        {
            return priceAverage.Where(dictionEntry =>
            {
                return ids.Any(item =>
                {
                    return item == dictionEntry.Key.ToString();
                });
            }).ToDictionary(i => i.Key, i => i.Value);
        }
    }
}
