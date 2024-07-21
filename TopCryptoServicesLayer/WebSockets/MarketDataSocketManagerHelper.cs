using TopCrypto.ServicesLayer.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Helpers;
using System;
using Newtonsoft.Json.Linq;
using TopCrypto.ServicesLayer.WebSockets.Interfaces;
using TopCrypto.ServicesLayer.CoinInfo.Models;
using TopCrypto.ServicesLayer.CoinInfo.Interfaces;
using TopCrypto.DataLayer.Services.CoinInfo.Models;
using TopCrypto.ServicesLayer.WebSockets.Models;

namespace TopCrypto.ServicesLayer.WebSockets
{
    public class MarketDataSocketManagerHelper : IMarketDataSocketManagerHelper
    {
        private readonly AbstractComparisonService<CoinInfoComparisonDTO, CoinInfoDTO> _coinDataComparisonService;
        private readonly ISocketDataWrapperHelpers _socketDataWrapperHelper;
        private readonly ICryptoCurencyPriceSaver _cryptoCurencyPriceSaver;
        private JsonHelper _jsonHelper;

        public MarketDataSocketManagerHelper(
            AbstractComparisonService<CoinInfoComparisonDTO, CoinInfoDTO> coinDataComparisonService
            , ISocketDataWrapperHelpers socketDataWrapperHelper
            , ICryptoCurencyPriceSaver cryptoCurencyPriceSaver
            , JsonHelper jsonHelper)
        {
            this._coinDataComparisonService = coinDataComparisonService;
            this._socketDataWrapperHelper = socketDataWrapperHelper;
            this._cryptoCurencyPriceSaver = cryptoCurencyPriceSaver;
            this._jsonHelper = jsonHelper;
        }

        public async Task OnMessageReceive(
                string IdInList
                , SocketManagerRecevidMessageDTO messageDTO
                , SocketDTO socketDTO
                , ISocketManager _socketManager)
        {

            if (messageDTO.VisibleCount > 0)
            {
                socketDTO.CountOfCoinPrice = messageDTO.VisibleCount;
            }

            var visibleCurrency = _coinDataComparisonService.GetLastResult().Take(messageDTO.VisibleCount);
            var weekPriceAverageFiltered = _cryptoCurencyPriceSaver
                .GetWeekPriceAverage(visibleCurrency.Select(item => item.Id));

            var json = _socketDataWrapperHelper.WrapForCoinData(
                JsonConvert.SerializeObject(visibleCurrency)
                , _coinDataComparisonService.GetLastResult().Count
                , JsonConvert.SerializeObject(weekPriceAverageFiltered));


            await _socketManager.SendMessageAsync(IdInList
               , socketDTO.Socket
               , json);
        }


        public SocketManagerRecevidMessageDTO TryParseReponseString(string wsMessage)
        {
            if (String.IsNullOrWhiteSpace(wsMessage)) return null;
            wsMessage = wsMessage.TrimEnd('\0');

            if (!_jsonHelper.IsValidJson(wsMessage, out JToken jToken)) return null;

            if (!(jToken is JObject jObject)) return null;

            jObject.TryGetValue("visiblePriceCount",
                StringComparison.InvariantCultureIgnoreCase,
                out JToken jtoken);

            if (jtoken == null) return null;

            int visibleCount = -1;
            Int32.TryParse(jtoken.ToString(), out visibleCount);

            if (visibleCount < 0) return null;

            return new SocketManagerRecevidMessageDTO()
            {
                VisibleCount = visibleCount
            };
        }
    }
}
