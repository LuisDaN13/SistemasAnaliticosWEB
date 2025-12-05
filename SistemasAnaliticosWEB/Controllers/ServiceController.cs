using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace SistemasAnaliticosWEB.Controllers
{
    public class ServiceController : Controller
    {
        [EnableRateLimiting("default")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
