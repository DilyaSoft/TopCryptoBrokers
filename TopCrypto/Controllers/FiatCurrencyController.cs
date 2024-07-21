using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TopCrypto.ServicesLayer.FiatCurrency.Interfaces;

namespace TopCrypto.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/FiatCurrency")]
    public class FiatCurrencyController : Controller
    {
        private IFiatCurrencyService _filterService;
        private IFiatCurrencyBackroundHelper _fiatCurrencyBackroundHelper;
        public FiatCurrencyController(IFiatCurrencyService filterService,
            IFiatCurrencyBackroundHelper fiatCurrencyBackroundHelper)
        {
            this._filterService = filterService;
            this._fiatCurrencyBackroundHelper = fiatCurrencyBackroundHelper;
        }

        [HttpPost("GetApiIds")]
        public async Task<JsonResult> GetApiIds()
        {
            return Json(await _filterService.GetApiIds());
        }

        #region Admin
        [HttpPost("GetDataBaseIds")]
        public async Task<JsonResult> GetDataBaseIds()
        {
            return Json(await _filterService.GetDataBaseIds());
        }

        [HttpPost("SaveDataBaseIds")]
        public async Task<ActionResult> SaveDataBaseIds([FromBody]string[] ids)
        {
            if (ids.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                return BadRequest("Entry cannot be null or empty");
            }

            await _filterService.SaveIdsToTable(ids);
            await _fiatCurrencyBackroundHelper.Execute();
            return Json(Ok());
        }
        #endregion
    }
}