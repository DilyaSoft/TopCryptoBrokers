using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TopCrypto.ServicesLayer.Interfaces;
using System.Linq;
using TopCrypto.ServicesLayer.WebSockets.Interfaces;
using TopCrypto.ServicesLayer.CoinInfo.Models;
using TopCrypto.DataLayer.Services.CoinInfo.Models;
using TopCrypto.ServicesLayer.FiatCurrency.Models;
using TopCrypto.DataLayer.Services.FiatCurrency.Models;
using TopCrypto.ServicesLayer.CoinInfo.Interfaces;
using TopCrypto.ServicesLayer.WebSockets.Models;

namespace TopCrypto.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MarketDataSocketController : Controller
    {
        private readonly ISocketManager _socketManager;
        private readonly AbstractComparisonService<CoinInfoComparisonDTO, CoinInfoDTO> _coinDataComparisonService;
        private readonly AbstractComparisonService<FiatCurrencyComparisonDTO, FiatCurrencyDTO> _fiatCurrencyComparisonService;
        private readonly ISocketDataWrapperHelpers _socketDataWrapperHelper;
        private readonly ICryptoCurencyPriceSaver _cryptoCurencyPriceSaver;

        public MarketDataSocketController(ISocketManager socketManager
            , AbstractComparisonService<CoinInfoComparisonDTO, CoinInfoDTO> coinDataComparisonService
            , AbstractComparisonService<FiatCurrencyComparisonDTO, FiatCurrencyDTO> fiatCurrencyComparisonService
            , ISocketDataWrapperHelpers socketDataWrapperHelper
            , ICryptoCurencyPriceSaver cryptoCurencyPriceSaver)
        {
            this._socketManager = socketManager;
            this._coinDataComparisonService = coinDataComparisonService;
            this._socketDataWrapperHelper = socketDataWrapperHelper;
            this._fiatCurrencyComparisonService = fiatCurrencyComparisonService;
            this._cryptoCurencyPriceSaver = cryptoCurencyPriceSaver;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("ConnectSocket")]
        public async Task ConnectSocket()
        {
            var context = Request.HttpContext;
            if (!context.WebSockets.IsWebSocketRequest) return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var id = _socketManager.AddSocket(new SocketDTO() { Socket = socket });

            await _socketManager.SendMessageAsync(id, socket,
                _socketDataWrapperHelper.WrapForFiatCurency(
                    JsonConvert.SerializeObject(_fiatCurrencyComparisonService.GetLastResult()))
                );

            var tenOrderedResult = _coinDataComparisonService.GetLastResult().Take(10);

            var weekPriceAverage = _cryptoCurencyPriceSaver
                .GetWeekPriceAverage(tenOrderedResult.Select(item => item.Id));

            await _socketManager.SendMessageAsync(id, socket,
                _socketDataWrapperHelper.WrapForCoinData(
                    JsonConvert.SerializeObject(tenOrderedResult)
                    , _coinDataComparisonService.GetLastResult().Count
                    , JsonConvert.SerializeObject(weekPriceAverage))
                );

            await _socketManager.Receive(id);
        }
    }
}