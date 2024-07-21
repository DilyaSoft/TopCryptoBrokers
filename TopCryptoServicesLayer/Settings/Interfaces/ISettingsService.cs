using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Settings.Models;
using TopCrypto.ServicesLayer.Settings.Models;

namespace TopCrypto.ServicesLayer.Settings.Interfaces
{
    public interface ISettingsService
    {
        Task AddOrUpdateSetting(string id, string value, string query);
        Task<SettingDTO> GetSettingByIdQuery(string id, string query);
        Task<SettingDTO> GetSettingByIdQueryWithDefaultEnglish(string id, string query);
        IList<SettingAdminDTO> GetSettingAdminDTOsForTemplatesAndUrls();
        IList<SettingAdminDTO> GetSettingAdminDTOsForTitles();
        Task<IList<SettingDTO>> GetSettingByListOfIdQueryWithDefaultEnglish(string query);
        SettingFromConfigDTO[] GetSettingFromConfigDTOs(string nameOfSection);
        string[] GetCultureFromConfig();
    }
}