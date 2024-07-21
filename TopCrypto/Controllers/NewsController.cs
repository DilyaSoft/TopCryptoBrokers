using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.News.Models;
using TopCrypto.Models.NewsViewModels;
using TopCrypto.Services;
using TopCrypto.ServicesLayer.News.Interfaces;

namespace TopCrypto.Controllers
{
    [Produces("application/json")]
    [Route("api/News")]
    public class NewsController : AbstractController
    {
        private INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            this._newsService = newsService;
        }

        #region RootApp
        [HttpGet("NewsTopTemplate")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult NewsTopTemplate()
        {
            return View("NewsTop");
        }

        [HttpGet("NewsDetailTemplate")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult NewsDetailTemplate()
        {
            return View("NewsDetail");
        }


        [HttpPost("NewsTop")]
        [AllowAnonymous]
        public JsonResult NewsTop()
        {
            return Json(_newsService.GetTopNews(GetCurrentCultureName()));
        }

        [HttpPost("NewsDetail")]
        [AllowAnonymous]
        public JsonResult NewsDetail(string link)
        {
            return Json(_newsService.GetNewsDetail(link, GetCurrentCultureName()));
        }        
        #endregion

        #region Admin
        [Authorize]
        [HttpGet("GetNewsListTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetNewsListTemplate()
        {
            return View("NewsListAdmin");
        }

        [Authorize]
        [HttpPost("GetNewsListData")]
        public async Task<JsonResult> GetNewsListData()
        {
            return Json(await _newsService.GetJsonNewsListForAdmin());
        }

        [Authorize]
        [HttpGet("GetAddEditTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GeAddEditTemplate()
        {
            return View("AddEditTemplate");
        }

        [Authorize]
        [HttpPost("AddNews")]
        public async Task<ActionResult> AddNews([FromBody] NewsAdminDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.ImgLink))
            {
                return BadRequest("ImgLink cannot be null");
            }
            else if (!model.DateForShowing.HasValue)
            {
                return BadRequest("DateForShowing cannot be null");
            }

            await _newsService.AddNews(model);

            return Json("Ok");
        }

        [Authorize]
        [HttpPost("GetNewsAdminDTO")]
        public async Task<JsonResult> GetNewsAdminDTO(int id)
        {
            return Json(await _newsService.GetAdminDTOById(id));
        }

        [Authorize]
        [HttpPost("UpdateNewsAdmin")]
        public async Task<ActionResult> UpdateNewsAdmin([FromBody]UpdateNewsAdminViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ImgLink))
            {
                return BadRequest("ImgLink cannot be null");
            }
            else if (!model.DateForShowing.HasValue)
            {
                return BadRequest("DateForShowing cannot be null");
            }

            await _newsService.UpdateNews(model.Id, model);
            return Json("Ok");
        }

        [Authorize]
        [HttpGet("GetEditNewsLocalizationTemplate")]
        [ResponseCache(VaryByHeader = "Cookie", Duration = 108000)]
        public ActionResult GetEditNewsLocalizationTemplate()
        {
            return View("EditNewsLocalization");
        }

        [Authorize]
        [HttpPost("GetLocalizationDTO")]
        public async Task<JsonResult> GetLocalizationDTO(int id, string culture)
        {
            return Json(await _newsService.GetLocalizationDTO(id, culture));
        }

        [Authorize]
        [HttpPost("UpdateLocalization")]
        public async Task<ActionResult> UpdateLocalization([FromBody] UpdateLocalizationViewModel model)
        {
            await _newsService.UpdateLocalization(model.Id, model.Culture, model);
            return Json("Ok");
        }
        #endregion
    }
}