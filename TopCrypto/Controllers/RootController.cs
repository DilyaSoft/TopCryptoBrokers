using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Language.Models;
using TopCrypto.Services;
using TopCrypto.ServicesLayer.Language.Interfaces;
using TopCrypto.ServicesLayer.Settings.Interfaces;

namespace TopCrypto.Controllers.Root
{
    public class RootController : AbstractController
    {
        private ILanguageService _languageService;
        private ISettingsService _settingsService;
        private readonly ILogger<RootController> _logger;
        public RootController(IStringLocalizer<RootController> localizer
            , ILanguageService languageService
            , ISettingsService settingsService
            , ILogger<RootController> logger)
        {
            this._languageService = languageService;
            this._settingsService = settingsService;
            this._logger = logger;
        }

        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public async Task<IActionResult> Index()
        {
            string acceptedLanguage = Request.Headers["Accept-Language"].ToArray()[0];
            var languageList = await _languageService.GetLanguageList();

            ViewData["selectedCulture"] = GetBrowserAcceptedLanguage(acceptedLanguage, languageList);
            ViewData["languages"] = languageList;
            ViewData["urlToTrade"] = (await _settingsService.GetSettingByIdQuery("url_to_navigate_to", null))?.Value;
            ViewData["linkToTitles"] = JsonConvert.SerializeObject(await _settingsService
                .GetSettingByListOfIdQueryWithDefaultEnglish(GetCurrentCultureName()));

           return View();
        }

        public string GetBrowserAcceptedLanguage(string acceptedLanguage, IList<LanguageDTO> languageList)
        {
            if (string.IsNullOrWhiteSpace(acceptedLanguage)
                || languageList == null
                || languageList.Count == 0) return string.Empty;

            var selectedCulture = string.Empty;
            var splitted = acceptedLanguage.Split(",");

            for (var i = 0; i < splitted.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(splitted[i])) continue;

                var dto = languageList.FirstOrDefault(x => x.Culture.ToLower().Trim().Contains(splitted[i].ToLower().Trim()));

                if (dto == null) continue;

                selectedCulture = dto.Culture;
                break;
            }

            return selectedCulture;
        }
    }
}