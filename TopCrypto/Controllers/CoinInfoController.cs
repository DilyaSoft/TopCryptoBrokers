using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinGraph.Models;
using TopCrypto.ServicesLayer.CoinInfo.Interfaces;
using TopCrypto.ServicesLayer.CoinMarketAndIds.Interfaces;

namespace TopCrypto.Controllers.Coininfo
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/CoinInfo")]
    public class CoinInfoController : Controller
    {
        private ICoinInfoService _coinInfoService;
        private ICoinMarketAndIds _coinMarketAndIds;
        private ICoinInfoBackroundHelper _coinDataBackroundHelper;

        public CoinInfoController(ICoinInfoService coinInfoService
            , ICoinMarketAndIds coinMarketAndIds
            , ICoinInfoBackroundHelper coinDataBackroundHelper)
        {
            this._coinInfoService = coinInfoService;
            this._coinMarketAndIds = coinMarketAndIds;
            this._coinDataBackroundHelper = coinDataBackroundHelper;
        }

        [HttpGet("PriceTemplate")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult Price()
        {
            return View("Price");
        }

        #region Admin
        [HttpPost("GetApiIds")]
        public async Task<JsonResult> GetApiIds()
        {
            return Json(await _coinMarketAndIds.GetListOfIdsWithMarkets());
        }

        [HttpPost("GetDataBaseIds")]
        public async Task<JsonResult> GetDataBaseIds()
        {
            return Json(await _coinInfoService.GetDataBaseIds());
        }

        [HttpPost("SaveDataBaseIds")]
        public async Task<ActionResult> SaveDataBaseIds([FromBody]CryptoCurrencyDTO[] ids)
        {
            if (ids.Any(x => string.IsNullOrWhiteSpace(x.Abb)))
            {
                return BadRequest("Abb cannot be null");
            }

            await _coinInfoService.ClearAndInsertNewCryptoCurrency(ids);
            await _coinDataBackroundHelper.Execute(false);
            return Json(Ok());
        }

        [HttpGet("CoinInfoTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult Index()
        {
            return View("CoinInfoSettings");
        }
        #endregion
    }
}