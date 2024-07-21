using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinGraph;
using TopCrypto.DataLayer.Services.CoinGraph.Interfaces;
using TopCrypto.DataLayer.Services.CoinGraph.Models;
using TopCrypto.DataLayer.Services.Helpers;
using TopCrypto.DataLayer.Services.Settings.Interfaces;
using TopCrypto.ServicesLayer.CoinGraph.Interfaces;
using TopCrypto.ServicesLayer.DBLogging.Interfaces;

namespace TopCrypto.ServicesLayer.CoinGraph
{
    public class CoinInfoGraphService : ICoinInfoGraphService
    {
        private ICoinInfoGraphDataService _coinInfoGraphDataService;
        private EndPointHelper _endPointHelper;
        private ISettingsDataService _settingsDataService;
        private IConfiguration _config;
        private IDBLoggingService _dbLoggingService;
        private ILogger<CoinInfoGraphService> _logger;

        public CoinInfoGraphService(ICoinInfoGraphDataService coinInfoGraphDataService,
            EndPointHelper endPointHelper,
            IConfiguration config,
            ISettingsDataService settingsDataService,
            IDBLoggingService dbLoggingService,
            ILogger<CoinInfoGraphService> logger)
        {
            this._coinInfoGraphDataService = coinInfoGraphDataService;
            this._endPointHelper = endPointHelper;
            this._config = config;
            this._settingsDataService = settingsDataService;
            this._dbLoggingService = dbLoggingService;
            this._logger = logger;
        }

        public async Task<bool> IsCacheFresh()
        {
            return await _settingsDataService.IsCacheFresh();
        }

        public async Task UpdateDBCacheForAll()
        {
            var currencies = await _coinInfoGraphDataService.GetCryptoСurrency();
            var settingDTO = await _settingsDataService.GetCoinSetting();

            if (currencies == null || currencies.Count == 0) return;

            var currencyIndex = 0;
            var timeIndex = TimePeriod.DAY;
            if (!String.IsNullOrWhiteSpace(settingDTO.LastSavedCoinId) &&
               settingDTO.LastTimeType.HasValue)
            {
                currencyIndex = currencies.FindIndex(item => item.Id.ToString() == settingDTO.LastSavedCoinId);
                timeIndex = TimePeriod.DAY;

                if (currencyIndex == -1)
                {
                    await _settingsDataService.UpdateCoinSettings(null, null);
                    currencyIndex = 0;
                    timeIndex = TimePeriod.DAY;
                }
                else if (timeIndex == TimePeriod.YEAR)
                {
                    currencyIndex++;
                    timeIndex = TimePeriod.DAY;
                }
                else
                {
                    timeIndex++;
                }
            }

            for (; currencyIndex < currencies.Count; currencyIndex++)
            {
                for (; timeIndex <= TimePeriod.YEAR; timeIndex++)
                {
                    try
                    {
                        var startDate = await _coinInfoGraphDataService.GetLastStartDateAndDeleteRow(
                            currencies[currencyIndex].Id.ToString()
                            , timeIndex
                            , currencies[currencyIndex].MarketId);
                        var result = await GetCoinApiDTOFromApi(
                            currencies[currencyIndex].MarketId
                            , timeIndex
                            , startDate);
                        await _coinInfoGraphDataService.UpdateCacheData(result
                            , currencies[currencyIndex].Id.ToString()
                            , timeIndex
                            , currencies[currencyIndex].MarketId);
                        await _settingsDataService.UpdateCoinSettings(currencies[currencyIndex].Id.ToString(), timeIndex);
                    }
                    catch (Exception e)
                    {
                        //await _dbLoggingService.WriteLog(e,
                        //    String.Format("UpdateDBCacheForAll currencyId:{0}"
                        //        , currencies[currencyIndex].Id));

                        _logger.LogError(e,String.Format("UpdateDBCacheForAll currencyId:{0}", currencies[currencyIndex].Id));
                    }
                }
                timeIndex = TimePeriod.DAY;
            }

            await _settingsDataService.UpdateCoinSettings(null, null);
            await _settingsDataService.UpdateSettingsCacheUpdateTime();
        }

        /*
        public async Task UpdateDBCacheForOneEntry(string coinId, TimePeriod timeType)
        {
            var currency = await _coinInfoGraphDataService.GetCryptoСurrencyById(coinId);

            if (currency == null) return;

            var startDate = await _coinInfoGraphDataService
                .GetLastStartDateAndDeleteRow(currency.Id.ToString(), timeType, currency.MarketId);
            var result = await GetCoinApiDTOFromApi(currency.MarketId, timeType, startDate);
            await _coinInfoGraphDataService.UpdateCacheData(result
                , coinId
                , timeType
                , currency.MarketId);
        }
        */

        public async Task<List<CoinInfoGraphDTO>> GetCoinApiDTOFromApi(string marketId
            , TimePeriod timeType
            , DateTime dataStart)
        {
            string param_time_start = dataStart.ToString("yyyy-MM-ddT00:00:00");

            string param_period_id = "";
            switch (timeType)
            {
                case (TimePeriod.DAY):
                    param_period_id = "1DAY";
                    break;
                case (TimePeriod.MONTH):
                    param_period_id = "1MTH";
                    break;
                case (TimePeriod.YEAR):
                    param_period_id = "1YRS";
                    break;
            }

            var queryParams = new Dictionary<string, string>() {
                //{ "time_end",  param_time_end}
                { "time_start",  param_time_start}
                ,{ "period_id", param_period_id}
                //,{ "limit", "3" }
                ,{ "limit", "100000" }
            };

            var headers = new Dictionary<string, string>() {
                { "X-CoinAPI-Key", _config.GetSection("X-CoinAPI-Key").Value}
            };

            var httpResponseMessage = await _endPointHelper.GetResponse(String.Format("https://rest.coinapi.io/v1/ohlcv/{0}/history", marketId), queryParams
                , headers);

            var json = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(json);
            }

            JArray jArray = JArray.Parse(json);

            var list = new List<CoinInfoGraphDTO>();
            foreach (var item in jArray)
            {
                var dto = JsonConvert.DeserializeObject<CoinInfoGraphDTO>(item.ToString());
                list.Add(dto);
            }

            return list;
        }

        public async Task<List<CoinInfoGraphDTO>> GetCoinApiDTOFromCache(string coinId
            , string timeType
            , DateTime? startDate
            , DateTime? endDate)
        {
            if (string.IsNullOrWhiteSpace(coinId) ||
                string.IsNullOrWhiteSpace(timeType) ||
                !Enum.TryParse(typeof(TimePeriod), timeType.ToUpper(), out object result))
            {
                return new List<CoinInfoGraphDTO>();
            }

            switch ((TimePeriod)result)
            {
                case TimePeriod.DAY:
                    {
                        if (startDate.HasValue)
                        {
                            startDate = startDate.Value.Date;
                        }

                        if (endDate.HasValue)
                        {
                            endDate = endDate.Value.Date;
                        }

                        break;
                    }
                case TimePeriod.MONTH:
                    {
                        if (startDate.HasValue)
                        {
                            var val = startDate.Value;
                            startDate = new DateTime(val.Year, 1, 1).Date;
                        }

                        if (endDate.HasValue)
                        {
                            var val = endDate.Value;
                            endDate = new DateTime(val.Year, 1, 1).Date;
                        }
                        else
                        {
                            var val = startDate.Value;
                            endDate = new DateTime(val.Year, 12, 1).Date;
                        }

                        break;
                    }
            }
            return await _coinInfoGraphDataService.GetCoinInfoGraphDTOs(coinId
                , (TimePeriod)result
                , startDate
                , endDate);
        }
    }
}
