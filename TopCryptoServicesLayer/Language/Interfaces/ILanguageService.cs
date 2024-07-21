using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Language.Models;

namespace TopCrypto.ServicesLayer.Language.Interfaces
{
    public interface ILanguageService
    {
        Task<IList<LanguageDTO>> GetLanguageList();
    }
}