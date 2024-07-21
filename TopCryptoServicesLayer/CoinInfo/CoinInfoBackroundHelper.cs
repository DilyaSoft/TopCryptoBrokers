using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinInfo.Models;
using TopCrypto.ServicesLayer.CoinInfo.Interfaces;
using TopCrypto.ServicesLayer.CoinInfo.Models;
using TopCrypto.ServicesLayer.DBLogging.Interfaces;
using TopCrypto.ServicesLayer.Interfaces;
using TopCrypto.ServicesLayer.SocketServices.WebSockets;
using TopCrypto.ServicesLayer.WebSockets.Interfaces;

namespace TopCrypto.ServicesLayer.CoinInfo
{
    public class CoinInfoBackroundHelper : ICoinInfoBackroundHelper
    {
        private readonly ISocketManager _socketManager;
        private readonly ICoinInfoService _marketService;
        private readonly AbstractComparisonService<CoinInfoComparisonDTO, CoinInfoDTO> _marketDataComparisonService;
        private readonly IDBLoggingService _dbLoggingService;
        private readonly ISocketDataWrapperHelpers _socketDataWrapperHelper;
        private readonly ICryptoCurencyPriceSaver _cryptoCurencyPriceSaver;

        private static volatile int _isFirstIteration = 1;
        private bool IsFirstIteration
        {
            get
            {

                return 1 == Interlocked.CompareExchange(ref _isFirstIteration, 1, 1);
            }
            set
            {
                if (!value)
                {
                    Interlocked.CompareExchange(ref _isFirstIteration, 0, 1);
                }
                else
                {
                    Interlocked.CompareExchange(ref _isFirstIteration, 1, 0);
                }
            }
        }

        public CoinInfoBackroundHelper(ISocketManager socketManager
            , ICoinInfoService marketService
            , AbstractComparisonService<CoinInfoComparisonDTO, CoinInfoDTO> marketDataComparisonService
            , IDBLoggingService dbLoggingService
            , ISocketDataWrapperHelpers socketDataWrapperHelper
            , ICryptoCurencyPriceSaver cryptoCurencyPriceSaver)
        {
            this._socketManager = socketManager;
            this._marketService = marketService;
            this._marketDataComparisonService = marketDataComparisonService;
            this._dbLoggingService = dbLoggingService;
            this._socketDataWrapperHelper = socketDataWrapperHelper;
            this._cryptoCurencyPriceSaver = cryptoCurencyPriceSaver;
        }

        public async Task Execute(bool saveResult = true)
        {
            var marketData = await _marketService.GetFilteredData();

            var comparisonData = _marketDataComparisonService.GetDiffrencess(marketData);

            if (saveResult)
            {
                await _cryptoCurencyPriceSaver.SaveNewResult(
                   comparisonData.Where((item) => { return item.Code != SocketComparisonEnum.NotModified; })
                   .Select(item => (CoinInfoDTO)item)
                   .ToList<CoinInfoDTO>());
            }

            var readonlySocketDTOs = _socketManager.GetReadonlySocketsDTO();

            var averagePrices = _cryptoCurencyPriceSaver.GetWeekPriceAverage(out bool isFromCache, false);

            if (comparisonData.Count != 0)
            {
                foreach (var entry in readonlySocketDTOs)
                {
                    var entryVal = entry.Value;
                    var comparisonDataForspecificSocket = comparisonData
                        .Where((item) => { return item.Code == SocketComparisonEnum.NotModified; })
                        .Take(entryVal.CountOfCoinPrice)
                        .ToList();
                    comparisonDataForspecificSocket.AddRange(comparisonData
                        .Where((item) => { return item.Code != SocketComparisonEnum.NotModified; }));

                    Dictionary<int, List<double>> priceAverageForSocket = null;
                    if (!isFromCache || IsFirstIteration)
                    {
                        priceAverageForSocket = _cryptoCurencyPriceSaver
                    .GetWeekPriceAverage(
                            comparisonDataForspecificSocket.Select(item => item.Id), averagePrices);
                    }

                    comparisonDataForspecificSocket = comparisonDataForspecificSocket
                    .Where((item) => { return item.Code != SocketComparisonEnum.NotModified; })
                    .ToList();

                    if (comparisonDataForspecificSocket.Count() == 0 && isFromCache
                        && !IsFirstIteration) continue;

                    await _socketManager.SendMessageAsync(entry.Key, entryVal.Socket,
                    _socketDataWrapperHelper.WrapForCoinData(
                        JsonConvert.SerializeObject(comparisonDataForspecificSocket)
                        , comparisonData.Where(x => x.Code == SocketComparisonEnum.Added
                        || x.Code == SocketComparisonEnum.NotModified
                        || x.Code == SocketComparisonEnum.Updated).Count()
                        , JsonConvert.SerializeObject(priceAverageForSocket)));
                }
            }

            IsFirstIteration = false;
        }
    }
}
