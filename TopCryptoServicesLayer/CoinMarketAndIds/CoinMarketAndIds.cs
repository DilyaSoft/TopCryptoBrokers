using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinIds.Interfaces;
using TopCrypto.DataLayer.Services.CoinMarket.Interfaces;
using TopCrypto.DataLayer.Services.CustomCache;
using TopCrypto.ServicesLayer.CoinMarketAndIds.Interfaces;
using TopCrypto.ServicesLayer.CoinMarketAndIds.Models;

namespace TopCrypto.ServicesLayer.CoinMarketAndIds
{
    public class CoinMarketAndIds : ICoinMarketAndIds
    {
        private ICoinIdsDataService _coinIdsDataService;
        private ICoinMarketDataService _coinMarketDataService;
        private InternalCache _cache;
        private static object locked = new object();
        private int CACHE_TIME_SECONDS = 60 * 60 * 1000; // 1 hour

        public CoinMarketAndIds(ICoinIdsDataService coinIdsDataService
            , ICoinMarketDataService coinMarketDataService
            , InternalCache cache)
        {
            this._coinIdsDataService = coinIdsDataService;
            this._coinMarketDataService = coinMarketDataService;
            this._cache = cache;
        }

        public Task<IEnumerable<CoinIdsListDTO>> GetListOfIdsWithMarkets()
        {
            lock (locked)
            {
                if (_cache.Get(InternalCacheKeys.CoinMarketAndIds)
                    is IEnumerable<CoinIdsListDTO> obj) { return Task.FromResult(obj); }

                var result = GetDictionaryIgnoreCache();
                result.Wait();

                var cloneOfDictionary = result.Result.Select(entry => (CoinIdsListDTO)entry.Clone());

                _cache.Add(InternalCacheKeys.CoinMarketAndIds
                    , cloneOfDictionary
                    , this.CACHE_TIME_SECONDS);

                return result;
            }
        }

        public async Task CleanDictinaryCache()
        {
            var result = await GetDictionaryIgnoreCache();
            lock (locked)
            {
                _cache.Add(InternalCacheKeys.CoinMarketAndIds
                    , result
                    , this.CACHE_TIME_SECONDS);
            }
        }

        private async Task<IEnumerable<CoinIdsListDTO>> GetDictionaryIgnoreCache()
        {
            var tsk1 = _coinIdsDataService.GetAllCoinsIDFromDB();
            var tsk2 = _coinMarketDataService.GetDataFromDBCache();

            await tsk1;
            await tsk2;

            var idsLst = tsk1.Result;
            var marketLst = tsk2.Result;

            var list = new List<CoinIdsListDTO>();
            foreach (var item in idsLst)
            {
                list.Add(
                    new CoinIdsListDTO(item)
                    {
                        Markets = marketLst.Where(x => string.Equals(x.AssetIdBase, item.Symbol))
                            .Select(x => x.SymbolId).ToList()
                    });
            }

            return list;
        }
    }
}
