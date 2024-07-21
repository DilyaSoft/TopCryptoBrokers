using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.News.Models;

namespace TopCrypto.DataLayer.Services.News.Interfaces
{
    public interface INewsDataService
    {
        Task<List<NewsTopDTO>> GetTopNews(int newsCount, string cultureName);
        Task<NewsDetailDTO> GetNewsDetail(string link, string cultureName);

        Task<string> GetJsonNewsListForAdmin();
        Task AddNews(NewsAdminDTO model);
        Task<NewsAdminDTO> GetAdminDTOById(int id);
        Task UpdateNews(int id, NewsAdminDTO dto);

        Task<NewsLocalizationDTO> GetLocalizationDTO(int id, string cultureName);
        Task UpdateLocalization(int id, string cultureName, NewsLocalizationDTO dto);
    }
}