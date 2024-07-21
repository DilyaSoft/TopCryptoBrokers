using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Settings.Models;
using TopCrypto.ServicesLayer.Settings.Interfaces;
using TopCrypto.ServicesLayer.Settings.Models;

namespace TopCrypto.Controllers.Settings
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : Controller
    {
        private ISettingsService _settingsService;
        private IConfiguration _config;
        public SettingsController(ISettingsService settingsService, IConfiguration config)
        {
            this._settingsService = settingsService;
            this._config = config;
        }

        [HttpPost("GetSettingsTemplatesAndURlsAdmin")]
        public ActionResult GetSettingsTemplatesAndURlsAdmin()
        {
            return Json(_settingsService.GetSettingAdminDTOsForTemplatesAndUrls());
        }

        [HttpPost("GetSettingsTitlesAdmin")]
        public ActionResult GetSettingsTitlesAdmin()
        {
            return Json(_settingsService.GetSettingAdminDTOsForTitles());
        }

        [HttpGet("GetAllSettingsTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetAllSettingsTemplate()
        {
            return View("AllSettingsTemplate");
        }


        [HttpGet("GetSettingTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetSettingsTemplate()
        {
            return View("SettingsTemplate");
        }

        [HttpPost("GetSettingByIdQuery")]
        public async Task<ActionResult> GetSettingByIdQuery([FromBody]SettingAdminDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.Id))
            {
                return BadRequest("id is null or empty");
            }
            return Json(await _settingsService.GetSettingByIdQuery(model.Id, model.Query));
        }

        [HttpPost("UpdateSettingByIdQuery")]
        public async Task<ActionResult> UpdateSettingByIdQuery([FromBody]SettingDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.Id))
            {
                return BadRequest("id is null or empty");
            }

            if (string.IsNullOrWhiteSpace(model.Value))
            {
                model.Value = null;
            }

            if (model.Query != null)
            {
                var templateIds = _settingsService.GetSettingFromConfigDTOs("templateIdsSettings");
                if (!templateIds.Any(x => string.Equals(x.Name, model.Id, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var linksTitle = _settingsService.GetSettingFromConfigDTOs("linksTitleSettings");
                    if (!linksTitle.Any(x => string.Equals(x.Name, model.Id, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        return BadRequest("setting is not allowed");
                    }
                }

                var cultures = _settingsService.GetCultureFromConfig();
                if (!cultures.Any(x => string.Equals(x, model.Query, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return BadRequest("setting is not allowed");
                }
            }
            else
            {
                var adminSettings = _settingsService.GetSettingFromConfigDTOs("adminSettings");
                if (!adminSettings.Any(x => string.Equals(x.Name, model.Id, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return BadRequest("setting is not allowed");
                }
            }

            await _settingsService.AddOrUpdateSetting(model.Id, model.Value, model.Query);

            return Json("Ok");
        }
    }
}