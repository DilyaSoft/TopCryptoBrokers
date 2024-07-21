using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
//using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CustomCache;
using TopCrypto.DataLayer.Services.News.Interfaces;
using TopCrypto.DataLayer.Services.News.Models;
using TopCrypto.ServicesLayer.News.Interfaces;

namespace TopCrypto.ServicesLayer.News
{
    public class NewsService : INewsService
    {
        private INewsDataService _newsDataService;
        private IConfiguration _configuration;
        private InternalCache _internalCache;
        private ILogger<NewsService> _logger;

        private readonly int CACHE_SECONDS = 60 * 5;
        private readonly string newsDetailCacheQueryPattern = "news_detail_id:{0};culture:{1}";
        private static object _lockObject = new object();
        private static object _lockObjectDetail = new object();

        public NewsService(INewsDataService newsDataService
            , InternalCache internalCache
            , IConfiguration configuration
            , ILogger<NewsService> logger)
        {
            this._newsDataService = newsDataService;
            this._internalCache = internalCache;
            this._configuration = configuration;
            this._logger = logger;
        }

        #region Root app
        public List<NewsTopDTO> GetTopNews(string culture)
        {
            lock (_lockObject)
            {
                var news = _internalCache.Get(InternalCacheKeys.News, culture);
                if (news != null)
                {
                    return (List<NewsTopDTO>)news;
                }

                var newsTask = _newsDataService.GetTopNews(
                    Int32.Parse(_configuration.GetSection("NewsCount").Value), culture);
                newsTask.Wait();
                news = newsTask.Result;
                _internalCache.Add(InternalCacheKeys.News, news, CACHE_SECONDS, culture);

                return (List<NewsTopDTO>)news;
            }
        }

        public NewsDetailDTO GetNewsDetail(string link, string culture)
        {
            lock (_lockObjectDetail)
            {
                var cacheQuery = string.Format(newsDetailCacheQueryPattern, link, culture);
                var news = _internalCache.Get(InternalCacheKeys.News, cacheQuery);
                if (news != null)
                {
                    return (NewsDetailDTO)news;
                }

                var newsTask = _newsDataService.GetNewsDetail(link, culture);
                newsTask.Wait();
                news = newsTask.Result;
                _internalCache.Add(InternalCacheKeys.News, news, CACHE_SECONDS, cacheQuery);

                return (NewsDetailDTO)news;
            }
        }
        #endregion

        #region Admin
        public async Task<string> GetJsonNewsListForAdmin()
        {
            return await _newsDataService.GetJsonNewsListForAdmin();
        }

        public async Task AddNews(NewsAdminDTO model)
        {
            await _newsDataService.AddNews(model);
        }

        public async Task<NewsAdminDTO> GetAdminDTOById(int id)
        {
            return await _newsDataService.GetAdminDTOById(id);
        }


        public async Task UpdateNews(int id, NewsAdminDTO model)
        {
            await _newsDataService.UpdateNews(id, model);
            _internalCache.CleanStorageHard(InternalCacheKeys.News);
        }

        public async Task<NewsLocalizationDTO> GetLocalizationDTO(int id, string culture)
        {
            return await _newsDataService.GetLocalizationDTO(id, culture);
        }

        public async Task UpdateLocalization(int id, string culture, NewsLocalizationDTO model)
        {
            await _newsDataService.UpdateLocalization(id, culture, model);
            var cacheQuery = string.Format(newsDetailCacheQueryPattern, id, culture);
            _internalCache.CleanStorageHard(InternalCacheKeys.News);
        }
        #endregion
    }
}
