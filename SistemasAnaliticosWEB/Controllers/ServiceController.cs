using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemasAnaliticosWEB.Controllers
{
    public class ServiceController : Controller
    {
        // GET: ServiceController
        public ActionResult Index()
        {
            return View();
        }
    }
}
