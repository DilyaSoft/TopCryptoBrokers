using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinMarket.Interfaces;
using TopCrypto.DataLayer.Services.CoinMarket.Models;
using TopCrypto.DataLayer.Services.CustomCache;
using TopCrypto.DataLayer.Services.Helpers;
using TopCrypto.DataLayer.Services.Settings.Interfaces;
using TopCrypto.ServicesLayer.CoinMarket.Interfaces;

namespace TopCrypto.ServicesLayer.CoinMarket
{
    public class CoinMarketService : ICoinMarketService
    {
        private ICoinMarketDataService _coinMarketDataService;
        private IConfiguration _config;
        private EndPointHelper _endPointHelper;
        private ISettingsDataService _settingsDataService;
        private InternalCache _cache;

        public CoinMarketService(ICoinMarketDataService coinMarketDataService
            , IConfiguration config
            , EndPointHelper endPointHelper
            , ISettingsDataService settingsDataService
            , InternalCache cache)
        {
            this._coinMarketDataService = coinMarketDataService;
            this._config = config;
            this._endPointHelper = endPointHelper;
            this._settingsDataService = settingsDataService;
            this._cache = cache;
        }

        public async Task<bool> IsCacheFresh()
        {
            return await _settingsDataService.IsMarketCacheFresh();
        }

        public async Task UpdateDBCache()
        {
            var list = await GetDataFromEndPoint();
            await _coinMarketDataService.ClearAndInsertNewCache(list);
            await _settingsDataService.UpdateSettingsMarketUpdateTime();
            _cache.CleanStorageHard(InternalCacheKeys.CoinMarketAndIds);
        }

        private async Task<IList<CoinMarketDTO>> GetDataFromEndPoint()
        {
            var message = await _endPointHelper.GetResponse("https://rest.coinapi.io/v1/symbols"
                , new Dictionary<string, string>() { { "filter_symbol_id", "SPOT,USD" } }
                , new Dictionary<string, string>() { { "X-CoinAPI-Key", _config.GetSection("X-CoinAPI-Key").Value } });

            var json = await message.Content.ReadAsStringAsync();

            if (!message.IsSuccessStatusCode) { throw new Exception(json); }

            var array = JArray.Parse(json);

            var list = new List<CoinMarketDTO>();
            foreach (var entry in array)
            {
                if (String.Equals(entry["symbol_type"].ToString(), "SPOT", StringComparison.CurrentCultureIgnoreCase)
              && String.Equals(entry["asset_id_quote"].ToString(), "usd", StringComparison.CurrentCultureIgnoreCase))
                {
                    list.Add(JsonConvert.DeserializeObject<CoinMarketDTO>(entry.ToString()));
                }
            }

            return list;
        }
    }
}
