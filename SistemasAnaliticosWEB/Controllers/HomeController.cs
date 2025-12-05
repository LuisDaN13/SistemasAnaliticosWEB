using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SistemasAnaliticosWEB.Models;
using System.Diagnostics;

namespace SistemasAnaliticosWEB.Controllers
{
    public class HomeController : Controller
    {
        [EnableRateLimiting("default")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
