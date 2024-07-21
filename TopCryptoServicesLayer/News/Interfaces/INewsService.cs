using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.News.Models;

namespace TopCrypto.ServicesLayer.News.Interfaces
{
    public interface INewsService
    {
        List<NewsTopDTO> GetTopNews(string culture);
        NewsDetailDTO GetNewsDetail(string link, string culture);
        Task<string> GetJsonNewsListForAdmin();
        Task AddNews(NewsAdminDTO model);
        Task<NewsAdminDTO> GetAdminDTOById(int id);
        Task UpdateNews(int id, NewsAdminDTO model);
        Task<NewsLocalizationDTO> GetLocalizationDTO(int id, string culture);
        Task UpdateLocalization(int id, string culture, NewsLocalizationDTO model);
    }
}