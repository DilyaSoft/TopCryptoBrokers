using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CustomCache;
using TopCrypto.DataLayer.Services.FiatCurrency.Interfaces;
using TopCrypto.DataLayer.Services.FiatCurrency.Models;
using TopCrypto.DataLayer.Services.Helpers;
using TopCrypto.ServicesLayer.FiatCurrency.Interfaces;
using TopCrypto.ServicesLayer.Interfaces;

namespace TopCrypto.ServicesLayer.FiatCurrency
{
    public class FiatCurrencyService : AbstractFilterService<FiatCurrencyDTO, string>
        , IFiatCurrencyService
    {
        private IFiatCurrencyDataService _currencyDataService;
        private InternalCache _cache;
        private IConfiguration _config;
        private EndPointHelper _endPointService;
        
        protected static object locked = new object();
        protected int CACHE_TIME_SECONDS => 60 * 60;

        public FiatCurrencyService(IFiatCurrencyDataService fiatCurrentDataService,
            InternalCache cache,
            IConfiguration config,
            EndPointHelper endPointService) : base()
        {
            this._currencyDataService = fiatCurrentDataService;
            this._config = config;
            this._endPointService = endPointService;
            this._cache = cache;
        }

        public async Task<string[]> GetDataBaseIds()
        {
            return await _currencyDataService.GetDataBaseIds();
        }

        public async Task SaveIdsToTable(string[] ids)
        {
            await _currencyDataService.SaveIdsToTable(ids);
        }

        public async Task<IList<FiatCurrencyDTO>> GetFilteredData()
        {
            var marketPriceValues = await GetMarketValues();
            var moneyIds = await _currencyDataService.GetDataBaseIds();

            var fiatCurrencyDTO = GetFilteredDataSorting(marketPriceValues, moneyIds);

            //Add USD if not exists
            for (var i = 0; i < moneyIds.Length; i++)
            {
                if (!String.Equals(moneyIds[i].Trim(), "USD", StringComparison.CurrentCultureIgnoreCase)) continue;
                break;
            }
            fiatCurrencyDTO.Add(new FiatCurrencyDTO("USD", 1));

            return fiatCurrencyDTO;
        }


        protected override bool EqualsById(FiatCurrencyDTO t, string z)
        {
            return string.Equals(t.Name, z, StringComparison.CurrentCultureIgnoreCase);
        }

        public async Task<IList<FiatCurrencyDTO>> GetMarketValues()
        {
            var jsonResponse = await GetMarketValuesEURBase();

            if (!jsonResponse.success) throw new Exception("request is not succeed");

            var dictionary = jsonResponse.rates;
            var multiply = 1d;
            if (!string.Equals(jsonResponse.@base, "USD"))
            {
                multiply = dictionary["USD"];
            }

            IList<FiatCurrencyDTO> list = new List<FiatCurrencyDTO>();

            dictionary.Remove("USD");
            var enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Current;

                list.Add(new FiatCurrencyDTO(entry.Key, Math.Round(multiply / entry.Value, 6)));
            }
            list.Add(new FiatCurrencyDTO("USD", 1));
            dictionary["USD"] = 1;

            return list;
        }

        public async Task<string[]> GetApiIds()
        {
            var jsonResponse = await GetMarketValuesEURBase();

            if (!jsonResponse.success) throw new Exception("request is not succeed");
            var dictionary = jsonResponse.rates;

            var keys = new String[dictionary.Count];
            dictionary.Keys.CopyTo(keys, 0);

            return keys;
        }

        private Task<FiatJSonResponseDTO> GetMarketValuesEURBase()
        {
            lock (locked)
            {
                if (_cache.Get(InternalCacheKeys.FiatCurrencyDataService) is FiatJSonResponseDTO obj)
                    return Task.FromResult(obj);

                var result = GetFiatResponseModel();
                result.Wait();
                _cache.Add(InternalCacheKeys.FiatCurrencyDataService, (FiatJSonResponseDTO)result.Result.Clone(), this.CACHE_TIME_SECONDS);

                return result;
            }
        }

        private async Task<FiatJSonResponseDTO> GetFiatResponseModel()
        {
            var httpResponseMessage = await _endPointService.GetResponse(
                "http://data.fixer.io/api/latest?access_key="
                + _config.GetSection("FixerApiAccessKey").Value);

            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }
            return JsonConvert.DeserializeObject<FiatJSonResponseDTO>(content);
        }
    }
}
