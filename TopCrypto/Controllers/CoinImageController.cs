using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace TopCrypto.Controllers
{
    [Produces("application/json")]
    [Route("api/CoinImage")]
    public class CoinImageController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public CoinImageController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("GetImageById")]
        [ResponseCache(Duration = 60*60*24, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetApiIds(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetAsync(
                        String.Format("https://s2.coinmarketcap.com/static/img/coins/32x32/{0}.png", id));

                    if (result.Content.Headers.ContentType.MediaType == "image/png")
                    {
                        using (Stream receiveStream = await result.Content.ReadAsStreamAsync())
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                receiveStream.CopyTo(ms);
                                return File(ms.ToArray(), "image/png");
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            var webrootPath = _hostingEnvironment.WebRootPath;

            var mokeFile = Path.GetFullPath(Path.Combine(Path.Combine(webrootPath, "images"), "coinMoke.png"));

            using (Stream receiveStream = new FileStream(mokeFile, FileMode.Open, FileAccess.Read))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    receiveStream.CopyTo(ms);
                    return File(ms.ToArray(), "image/png");
                }
            }
        }
    }
}