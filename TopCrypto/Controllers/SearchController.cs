using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TopCrypto.ServicesLayer.Search.Interfaces;

namespace TopCrypto.Controllers
{
    [Produces("application/json")]
    [Route("api/Search")]
    public class SearchController : Controller
    {
        public ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            this._searchService = searchService;
        }

        [HttpPost("Search")]
        public async Task<JsonResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return Json(null);

            return Json(await _searchService.GetSearchResult(query));
        }
    }
}