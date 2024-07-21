using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Search.Interfaces;
using TopCrypto.DataLayer.Services.Search.Models;
using TopCrypto.ServicesLayer.Search.Interfaces;

namespace TopCrypto.ServicesLayer.Search
{
    public class SearchService : ISearchService
    {
        private ISearchDataService _searchDataService;
        public SearchService(ISearchDataService searchDataService)
        {
            this._searchDataService = searchDataService;
        }

        public async Task<List<SearchDTO>> GetSearchResult(string query)
        {
            return await _searchDataService.GetSearchResult(query);
        }
    }
}
