using Microsoft.AspNetCore.Mvc;

namespace SistemasAnaliticosWEB.Controllers
{
    public class PrivacyController : Controller
    {
        [Route("privacy")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
