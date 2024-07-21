using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Search.Models;

namespace TopCrypto.DataLayer.Services.Search.Interfaces
{
    public interface ISearchDataService
    {
        Task<List<SearchDTO>> GetSearchResult(string queryPart);
    }
}