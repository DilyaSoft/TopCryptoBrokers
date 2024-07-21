using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Language.Interfaces;
using TopCrypto.DataLayer.Services.Language.Models;
using TopCrypto.ServicesLayer.Language.Interfaces;

namespace TopCrypto.ServicesLayer.Language
{
    public class LanguageService : ILanguageService
    {
        private ILanguageDataService _languageDataService;
        public LanguageService(ILanguageDataService languageDataService)
        {
            this._languageDataService = languageDataService;
        }

        public async Task<IList<LanguageDTO>> GetLanguageList()
        {
            return await _languageDataService.GetLanguageList();
        }
    }
}
