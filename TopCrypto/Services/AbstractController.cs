using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace TopCrypto.Services
{
    public class AbstractController : Controller
    {
        public string GetCurrentCultureName()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }
    }
}
