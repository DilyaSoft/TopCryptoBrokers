using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinGraph.Interfaces;
using TopCrypto.DataLayer.Services.CoinGraph.Models;
using TopCrypto.DataLayer.Services.CoinIds.Interfaces;
using TopCrypto.DataLayer.Services.CoinIds.Models;
using TopCrypto.DataLayer.Services.CoinInfo.Interfaces;
using TopCrypto.DataLayer.Services.CoinInfo.Models;
using TopCrypto.DataLayer.Services.CustomCache;
using TopCrypto.DataLayer.Services.Helpers;
using TopCrypto.ServicesLayer.CoinInfo.Interfaces;
using TopCrypto.ServicesLayer.Interfaces;

namespace TopCrypto.ServicesLayer.CoinInfo
{
    public class CoinInfoService :
        AbstractFilterService<CoinInfoDTO, string>
        , ICoinInfoService
    {
        private IConfiguration _configuration;
        private ICoinIdsDataService _coinInfoIdsDataService;
        private ICoinInfoGraphDataService _coinInfoGraphDataService;
        private ICoinInfoDataService _currencyDataService;
        

        private static object locked = new object();
        private int CACHE_TIME_SECONDS => 5 * 60 + 30;
        private EndPointHelper _endPointService;
        private InternalCache _cache;

        public CoinInfoService(
            ICoinInfoDataService mrktDataService
            , ICoinIdsDataService coinInfoIdsDataService
            , IConfiguration configuration
            , ICoinInfoGraphDataService coinInfoGraphDataService
            , EndPointHelper endPointService
            , InternalCache cache) : base()
        {
            this._coinInfoIdsDataService = coinInfoIdsDataService;
            this._configuration = configuration;
            this._coinInfoGraphDataService = coinInfoGraphDataService;
            this._currencyDataService = mrktDataService;
            this._endPointService = endPointService;
            this._cache = cache;
            
        }

        public async Task<IEnumerable<CryptoCurrencyDTO>> GetDataBaseIds()
        {
            return await _coinInfoGraphDataService.GetCryptoСurrency();
        }

        public async Task ClearAndInsertNewCryptoCurrency(CryptoCurrencyDTO[] arr)
        {
            await _currencyDataService.ClearAndInsertNewCryptoCurrency(arr);
            await UpdateCoinCache();
        }

        public async Task<IList<CoinIdsDTO>> GetAllCoinsID()
        {
            return await _coinInfoIdsDataService.GetAllCoinsIDFromDB();
        }

        public async Task UpdateCoinCache()
        {
            var coinIds = await _currencyDataService.GetAllCoinsIDFromDBExistedInDB();
            lock (locked)
            {
                if (_cache.Get(InternalCacheKeys.CoinInfoDataService) is IList<CoinInfoDTO> cachedCoins)
                {
                    var removedCoinDTO = new List<CoinInfoDTO>();
                    var existedCoinIds = new List<CoinIdNameDTO>();

                    foreach (var coin in cachedCoins)
                    {
                        var isCoinExist = false;
                        foreach (var coIdName in coinIds)
                        {
                            if (string.Equals(coin.Id, coIdName.Id, StringComparison.InvariantCultureIgnoreCase))
                            {
                                isCoinExist = true;
                                existedCoinIds.Add(coIdName);
                                break;
                            }
                        }
                        if (!isCoinExist)
                        {
                            removedCoinDTO.Add(coin);
                        }
                    }

                    foreach (var removedDTO in removedCoinDTO)
                    {
                        cachedCoins.Remove(removedDTO);
                    }

                    if (existedCoinIds.Count > 0)
                    {
                        var notExistedCoinIds = coinIds.Where(id => !existedCoinIds.Contains(id));

                        foreach (var coIdName in notExistedCoinIds)
                        {
                            cachedCoins.Add(new CoinInfoDTO()
                            {
                                Id = coIdName.Id,
                                Name = coIdName.Name,
                                PriceUsd = null,
                                PercentChange24h = null,
                                MarketCapUsd = null,
                                LastUpdated = null
                            });
                        }
                    }

                    _cache.AddUseOldTime(InternalCacheKeys.CoinInfoDataService
                        , cachedCoins
                        , this.CACHE_TIME_SECONDS);
                }
            }
        }

        public async Task<IList<CoinInfoDTO>> GetFilteredData()
        {
            var marketPriceValues = await GetAllCoinsWhichExistsInDBWithCache();
            var coinIds = await _currencyDataService.GetDataBaseIds();

            var filteredValues = GetFilteredDataSorting(marketPriceValues, coinIds);

            var bitcoin = marketPriceValues.FirstOrDefault(coin =>
                coin.Id == _configuration.GetSection("BitcoinIdInApi").Value);
            if (bitcoin != null && bitcoin.PriceUsd.HasValue)
            {
                foreach (var item in filteredValues)
                {
                    if (item.PriceUsd.HasValue)
                    {
                        item.PriceBtc = Math.Round((item.PriceUsd.Value / bitcoin.PriceUsd.Value), 6);
                    }
                }
            }

            return filteredValues;
        }

        protected override bool EqualsById(CoinInfoDTO t, string z)
        {
            return string.Equals(t.Id, z, StringComparison.CurrentCultureIgnoreCase);
        }


        private Task<IList<CoinInfoDTO>> GetAllCoinsWhichExistsInDBWithCache()
        {
            lock (locked)
            {
                if (_cache.Get(InternalCacheKeys.CoinInfoDataService) is IList<CoinInfoDTO> obj) return Task.FromResult(obj);

                var result = GetAllCoinsWhichExistsInDB();
                result.Wait();

                _cache.Add(InternalCacheKeys.CoinInfoDataService
                    , result.Result.Select(item => (CoinInfoDTO)item.Clone()).ToList()
                    , this.CACHE_TIME_SECONDS);

                return result;
            }
        }

        private async Task<IList<CoinInfoDTO>> GetAllCoinsWhichExistsInDB()
        {
            var idsArray = await GetDataBaseIds();
            var ids = string.Join(',', idsArray.Select(x => x.Id).ToArray());

            if (string.IsNullOrWhiteSpace(ids)) { return new List<CoinInfoDTO>(); }

            var queryParams = new Dictionary<string, string>() {
                { "id", ids }
                ,{ "convert", "USD" }
            };

            var headers = new Dictionary<string, string>() {
                { "X-CMC_PRO_API_KEY", _configuration.GetSection("X-CMC_PRO_API_KEY").Value}
            };

            var httpResponseMessage = await _endPointService.GetResponse("https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest", queryParams, headers);
            var json = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(json);
            }

            var jObject = JObject.Parse(json);

            var list = new List<CoinInfoDTO>();
            var jData = (JObject)jObject["data"];
            double? btcPrice = 0;
            foreach (var coin in jData.Properties())
            {
                var coinData = coin.First;
                var usd = coinData["quote"]?["USD"];
                var lastUpdatedDateTime = (DateTime)coinData["last_updated"];

                if (btcPrice == 0 && (string)coinData["id"] == "1")
                {
                    btcPrice = (double?)usd?["price"];
                }

                list.Add(new CoinInfoDTO()
                {
                    Id = (string)coinData["id"],
                    Name = (string)coinData["name"],
                    PriceUsd = (double?)usd?["price"],
                    PercentChange24h = (double?)usd?["percent_change_24h"],
                    MarketCapUsd = (double?)usd?["market_cap"],
                    LastUpdated = new DateTimeOffset(lastUpdatedDateTime).ToUnixTimeSeconds()
                });
            }

            foreach (var coin in list)
            {
                coin.PriceBtc = coin.PriceUsd / btcPrice;
            }

            return list;
        }
    }
}
