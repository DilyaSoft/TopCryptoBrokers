using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CustomCache;
using TopCrypto.DataLayer.Services.Settings.Interfaces;
using TopCrypto.DataLayer.Services.Settings.Models;
using TopCrypto.ServicesLayer.Settings.Interfaces;
using TopCrypto.ServicesLayer.Settings.Models;

namespace TopCrypto.ServicesLayer.Settings
{
    public class SettingsService : ISettingsService
    {
        private ISettingsDataService _settingsDataService;
        private IConfiguration _config;
        private InternalCache _cache;
        private int CACHE_TIME_SECONDS => 60 * 5;
        private static object _locked = new object();

        public SettingsService(ISettingsDataService settingsDataService
            , IConfiguration config
            , InternalCache cache)
        {
            this._config = config;
            this._settingsDataService = settingsDataService;
            this._cache = cache;
        }

        public async Task AddOrUpdateSetting(string id, string value, string query)
        {
            await _settingsDataService.AddOrUpdateSetting(id, value, query);
            _cache.CleanStorageHard(InternalCacheKeys.Settings);
        }

        public async Task<SettingDTO> GetSettingByIdQuery(string id, string query)
        {
            return await _settingsDataService.GetSettingByIdQuery(id, query);
        }

        public Task<SettingDTO> GetSettingByIdQueryWithDefaultEnglish(string id, string query)
        {
            lock (_locked)
            {
                var cacheQuery = string.Format("id:{0}query:{1} ", id, query);
                if (_cache.Get(InternalCacheKeys.Settings, cacheQuery) is SettingDTO obj)
                {
                    return Task.FromResult(obj);
                }

                var result = _settingsDataService.GetSettingByIdQueryWithDefaultEnglish(id, query);
                result.Wait();

                _cache.Add(InternalCacheKeys.Settings
                    , result.Result
                    , this.CACHE_TIME_SECONDS
                    , cacheQuery);

                return result;
            }
        }

        public Task<IList<SettingDTO>> GetSettingByListOfIdQueryWithDefaultEnglish(string query)
        {
            lock (_locked)
            {
                var cacheQuery = string.Format("settingByListOfId query:{0} ", query);
                if (_cache.Get(InternalCacheKeys.Settings, cacheQuery) is IList<SettingDTO> obj)
                {
                    return Task.FromResult(obj);
                }

                var linksTitleSettings = _config.GetSection("linksTitleSettings").AsEnumerable()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                    .Select(x => x.Value).ToList();

                var result = _settingsDataService.GetSettingByListOfIdQueryWithDefaultEnglish(linksTitleSettings, query);
                result.Wait();

                _cache.Add(InternalCacheKeys.Settings
                    , result.Result
                    , this.CACHE_TIME_SECONDS
                    , cacheQuery);

                return result;
            }
        }

        public IList<SettingAdminDTO> GetSettingAdminDTOsForTemplatesAndUrls()
        {
            var list = new List<SettingAdminDTO>();

            var cultures = GetCultureFromConfig();
            var templateIds = GetSettingFromConfigDTOs("templateIdsSettings");
            foreach (var template in templateIds)
            {
                foreach (var cultur in cultures)
                {
                    list.Add(new SettingAdminDTO() { Label = template.Label, Id = template.Name, Query = cultur });
                }
            }

            var adminSettings = GetSettingFromConfigDTOs("adminSettings");

            foreach (var setting in adminSettings)
            {
                list.Add(new SettingAdminDTO() { Label = setting.Label, Id = setting.Name });
            }

            return list;
        }

        public IList<SettingAdminDTO> GetSettingAdminDTOsForTitles()
        {
            var list = new List<SettingAdminDTO>();
            var linksTitleSettings = GetSettingFromConfigDTOs("linksTitleSettings");
            var cultures = GetCultureFromConfig();
            foreach (var linksTitle in linksTitleSettings)
            {
                foreach (var cultur in cultures)
                {
                    list.Add(new SettingAdminDTO() { Label = linksTitle.Label, Id = linksTitle.Name, Query = cultur });
                }
            }

            return list;
        }

        public SettingFromConfigDTO[] GetSettingFromConfigDTOs(string nameOfSection)
        {
            return _config
                .GetSection(nameOfSection)
                .Get<SettingFromConfigDTO[]>()
                .OrderBy(item => item.Label)
                .ToArray();
        }

        public string[] GetCultureFromConfig()
        {
            return _config
                .GetSection("supportedCultures")
                .Get<string[]>()
                .OrderBy(item => item)
                .ToArray();
        }
    }
}
