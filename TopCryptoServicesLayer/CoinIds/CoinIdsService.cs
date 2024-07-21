using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinIds.Interfaces;
using TopCrypto.DataLayer.Services.CoinIds.Models;
using TopCrypto.DataLayer.Services.CustomCache;
using TopCrypto.DataLayer.Services.Helpers;
using TopCrypto.DataLayer.Services.Settings.Interfaces;
using TopCrypto.ServicesLayer.CoinIds.Interfaces;

namespace TopCrypto.ServicesLayer.CoinIds
{
    public class CoinIdsService : ICoinIdsService
    {
        private ISettingsDataService _settingsDataService;
        private EndPointHelper _endPointService;
        private ICoinIdsDataService _coinIdsDataService;
        private IConfiguration _config;
        private InternalCache _cache;

        public CoinIdsService(ISettingsDataService settingsDataService
            , EndPointHelper endPointService
            , ICoinIdsDataService coinIdsDataService
            , IConfiguration config
            , InternalCache cache)
        {
            this._settingsDataService = settingsDataService;
            this._endPointService = endPointService;
            this._coinIdsDataService = coinIdsDataService;
            this._config = config;
            this._cache = cache;
        }

        public async Task<bool> IsCacheFresh()
        {
            return await _settingsDataService.IsIdsCacheFresh();
        }

        public async Task UpdateDBCacheForAll()
        {
            var lst = await GetAllCoinsIDFromEndPoint();
            await _coinIdsDataService.ClearAndInsertNewCache(lst);
            await _settingsDataService.UpdateSettingsIdsUpdateTime();
            _cache.CleanStorageHard(InternalCacheKeys.CoinMarketAndIds);
        }

        private async Task<IList<CoinIdsDTO>> GetAllCoinsIDFromEndPoint()
        {
            var queryParams = new Dictionary<string, string>() {
                { "listing_status", "active" }
            };

            var headers = new Dictionary<string, string>() {
                { "X-CMC_PRO_API_KEY", _config.GetSection("X-CMC_PRO_API_KEY").Value}
            };

            var httpResponseMessage = await _endPointService.GetResponse("https://pro-api.coinmarketcap.com/v1/cryptocurrency/map", queryParams, headers);
            var json = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(json);
            }

            var jObject = JObject.Parse(json);

            var list = new List<CoinIdsDTO>();
            var jData = (JArray)jObject["data"];
            foreach (var coinData in jData)
            {
                list.Add(new CoinIdsDTO()
                {
                    Id = (string)coinData["id"],
                    Name = (string)coinData["name"],
                    Symbol = (string)coinData["symbol"]
                });
            }

            return list;
        }
    }
}
