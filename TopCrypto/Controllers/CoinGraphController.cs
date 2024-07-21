using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TopCrypto.ServicesLayer.CoinGraph.Interfaces;

namespace TopCrypto.Controllers
{
    [Produces("application/json")]
    [Route("api/CoinGraph")]
    public class CoinGraphController : Controller
    {
        private ICoinInfoGraphService _coinInfoGraphService;
        public CoinGraphController(ICoinInfoGraphService coinInfoGraphService)
        {
            this._coinInfoGraphService = coinInfoGraphService;
        }

        [HttpPost("GetCoins")]
        public async Task<JsonResult> GetCoins(string coinId
            , string timeInterval
            , DateTime? startDate
            , DateTime? endDate)
        {
            return Json(await _coinInfoGraphService.GetCoinApiDTOFromCache(
                coinId
                , timeInterval
                , startDate
                , endDate));
        }
    }
}