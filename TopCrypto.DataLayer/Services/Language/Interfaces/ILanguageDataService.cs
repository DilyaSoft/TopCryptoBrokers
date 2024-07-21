using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Language.Models;

namespace TopCrypto.DataLayer.Services.Language.Interfaces
{
    public interface ILanguageDataService
    {
        Task<IList<LanguageDTO>> GetLanguageList();
    }
}