using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemasAnaliticosWEB.Models;

namespace SistemasAnaliticosWEB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
