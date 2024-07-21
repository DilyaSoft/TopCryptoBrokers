using Newtonsoft.Json;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.FiatCurrency.Models;
using TopCrypto.ServicesLayer.FiatCurrency.Interfaces;
using TopCrypto.ServicesLayer.FiatCurrency.Models;
using TopCrypto.ServicesLayer.Interfaces;
using TopCrypto.ServicesLayer.WebSockets.Interfaces;

namespace TopCrypto.ServicesLayer.FiatCurrency
{
    public class FiatCurrencyBackroundHelper : IFiatCurrencyBackroundHelper
    {
        private readonly ISocketManager _socketManager;
        private readonly IFiatCurrencyService _marketService;
        private readonly AbstractComparisonService<FiatCurrencyComparisonDTO, FiatCurrencyDTO> _marketDataComparisonService;
        private readonly ISocketDataWrapperHelpers _socketDataWrapperHelper;

        public FiatCurrencyBackroundHelper(ISocketManager socketManager
           , IFiatCurrencyService marketService
           , AbstractComparisonService<FiatCurrencyComparisonDTO, FiatCurrencyDTO> marketDataComparisonService
           , ISocketDataWrapperHelpers socketDataWrapperHelper)
        {
            this._socketManager = socketManager;
            this._marketService = marketService;
            this._marketDataComparisonService = marketDataComparisonService;
            this._socketDataWrapperHelper = socketDataWrapperHelper;
        }

        public async Task Execute() {
            var marketData = await _marketService.GetFilteredData();

            var comparisonData = _marketDataComparisonService.GetDiffrencess(marketData);

            if (comparisonData.Count != 0)
            {
                await _socketManager.SendMessageToAllAsync(
                    _socketDataWrapperHelper.WrapForFiatCurency(JsonConvert.SerializeObject(comparisonData)));
            }
        }

    }
}
