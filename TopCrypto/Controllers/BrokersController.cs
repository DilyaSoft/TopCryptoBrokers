using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Broker.Models;
using TopCrypto.Models.BrokersViewModels;
using TopCrypto.Services;
using TopCrypto.ServicesLayer.Interfaces;

namespace TopCrypto.Controllers.Brokers
{
    [Route("api/[controller]")]    
    public class BrokersController : AbstractController
    {
        #region RootApp
        private IBrokersService _brokersService;
        private readonly ILogger<BrokersController> _logger;


        public BrokersController(IBrokersService brokersService,
            ILogger<BrokersController> logger)
        {
            this._brokersService = brokersService;
            this._logger = logger;
        }

        [HttpGet("GetTopTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetTopTemplate()
        {
            return View("TopTemplate");
        }

        [HttpGet("GetTopIndexTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetIndexTemplate()
        {
            return View("TopIndexTemplate");
        }

        [HttpPost("GetTopIndex")]
        public JsonResult GetTopIndex()
        {
            return Json(_brokersService.GetTopIndex(GetCurrentCultureName()));
        }

        [HttpPost("GetTop")]
        public JsonResult GetTop([FromBody]int countItems)
        {
            return Json(_brokersService.GetTop(GetCurrentCultureName(), countItems));
        }

        [HttpGet("GetBrokerDetailTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetBrokerDetailTemplate()
        {
            return View("BrokerDetail");
        }

        [HttpPost("GetBroker")]
        public async Task<JsonResult> GetBroker(string name)
        {
            return Json(await _brokersService.GetBrokerById(name, GetCurrentCultureName()));
        }
        #endregion

        #region Admin
        [Authorize]
        [HttpGet("GetBrokerListTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetBrokerListTemplate()
        {
            return View("BrokerListAdmin");
        }

        [Authorize]
        [HttpPost("RemoveBroker")]
        public async Task<ActionResult> RemoveBroker(int id)
        {
            await _brokersService.RemoveBoker(id);
            return Json(await _brokersService.GetJsonBrokerListForAdmin());
        }

        [Authorize]
        [HttpPost("GetBrokerListData")]
        public async Task<JsonResult> GetBrokerListData()
        {
            return Json(await _brokersService.GetJsonBrokerListForAdmin());
        }

        [Authorize]
        [HttpGet("GetAddBrokerTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetAddBrokerTemplate()
        {
            return View("AddBrokerAdmin");
        }

        [Authorize]
        [HttpGet("GetEditBrokerLocalizationTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetEditBrokerLocalizationTemplate()
        {
            return View("EditBrokerLocalizationAdmin");
        }

        [Authorize]
        [HttpPost("AddBroker")]
        public async Task<ActionResult> AddBroker([FromBody] BrokerAdminDTO model)
        {
            if (String.IsNullOrWhiteSpace(model.OriginalName))
            {
                return BadRequest("OriginalName cannot be null");
            }

            await _brokersService.AddBrokerForAdmin(model);

            return Json("Ok");
        }

        [Authorize]
        [HttpPost("GetBrokerAdmin")]
        public async Task<JsonResult> GetBrokerAdmin(int id)
        {
            return Json(await _brokersService.GetBrokerAdminById(id));
        }

        [Authorize]
        [HttpPost("UpdateBrokerAdmin")]
        public async Task<ActionResult> UpdateBroker([FromBody]UpdateBrokerAdminViewModel broker)
        {
            if (string.IsNullOrWhiteSpace(broker.OriginalName))
            {
                return BadRequest("OriginalName cannot be null");
            }

            await _brokersService.UpdateBroker(broker.Id, broker);
            return Json("Ok");
        }

        [Authorize]
        [HttpPost("GetBrokerLocalizationData")]
        public async Task<JsonResult>
            GetBrokerLocalizationData(int id, string culture)
        {
            return Json(await _brokersService.GetBrokerLocalization(id, culture));
        }

        [Authorize]
        [HttpPost("UpdateBrokerLocalization")]
        public async Task<ActionResult>
            UpdateBrokerLocalizationData([FromBody] UpdateBrokerLocalizationViewModel localData)
        {
            if (string.IsNullOrWhiteSpace(localData.Name))
            {
                return BadRequest("Name cannot be null");
            }

            await _brokersService.UpdateBrokerLocalization(localData.Id, localData.Culture, localData);
            return Json("Ok");
        }
        #endregion
    }
}