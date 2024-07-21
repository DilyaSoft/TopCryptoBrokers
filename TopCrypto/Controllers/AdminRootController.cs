using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TopCrypto.Controllers
{
    //[Route("[controller]")]
    [Authorize]
    public class AdminRootController : Controller
    {
        //[Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

       // [Route("GetUserInfo")]
        [HttpPost]
        public JsonResult GetUserInfo()
        {
            var userName = User.FindFirst(ClaimTypes.Name).Value;
            return Json(Ok(userName));
        }
    }
}