using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Search.Models;

namespace TopCrypto.ServicesLayer.Search.Interfaces
{
    public interface ISearchService
    {
        Task<List<SearchDTO>> GetSearchResult(string query);
    }
}