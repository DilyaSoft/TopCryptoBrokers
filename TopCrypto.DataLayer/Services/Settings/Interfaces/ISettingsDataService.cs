using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinGraph;
using TopCrypto.DataLayer.Services.Settings.Models;

namespace TopCrypto.DataLayer.Services.Settings.Interfaces
{
    public interface ISettingsDataService
    {
        Task AddOrUpdateSetting(string id, string value, string query);
        Task<SettingDTO> GetSettingByIdQuery(string id, string query);
        Task<SettingDTO> GetSettingByIdQueryWithDefaultEnglish(string id, string query);
        Task<IList<SettingDTO>> GetSettingByListOfIdQueryWithDefaultEnglish(IList<string> ids, string query);

        Task<CoinSettingDTO> GetCoinSetting();
        Task<bool> IsCacheFresh();
        Task<bool> IsMarketCacheFresh();
        Task<bool> IsIdsCacheFresh();
        Task UpdateCoinSettings(string coinId, TimePeriod? timeType);
        Task UpdateSettingsMarketUpdateTime();
        Task UpdateSettingsCacheUpdateTime();
        Task UpdateSettingsIdsUpdateTime();
    }
}