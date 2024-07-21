using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using TopCrypto.Services;
using TopCrypto.ServicesLayer.Settings.Interfaces;

namespace TopCrypto.Controllers.Templates
{
    [Route("api/[controller]")]    
    public class TemplateController : AbstractController
    {
        private ISettingsService _settingsService;
        private IConfiguration _config;
        public TemplateController(ISettingsService settingsService, IConfiguration config)
        {
            this._settingsService = settingsService;
            this._config = config;
        }

        [HttpGet("GetHomeTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetHomeTemplate()
        {
            return View("HomeTemplate");
        }

        [HttpGet("GetCalculatorTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetCalculatorTemplate()
        {
            return View("CalculatorTemplate");
        }

        [HttpGet("GetStaticTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public async Task<ActionResult> GetStaticTemplate(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("setting is not allowed");
            }

            var templateIds = _settingsService.GetSettingFromConfigDTOs("templateIdsSettings");
            if (!templateIds.Any(x => string.Equals(x.Name, id, StringComparison.InvariantCultureIgnoreCase)))
            {
                return BadRequest("setting is not allowed");
            }

            var dto = await _settingsService.GetSettingByIdQueryWithDefaultEnglish(id, GetCurrentCultureName());

            ViewData["Value"] = dto?.Value;
            return View("StaticTemplate");
        }
    }
}