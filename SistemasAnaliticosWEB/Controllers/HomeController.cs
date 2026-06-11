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

        // En HomeController
        [Route("Error/404")]
        public IActionResult Error404()
        {
            Response.StatusCode = 404;
            return View("Error"); // Reutiliza tu vista de error
        }

        //-------------------------------------------------------------------------------------

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(errorViewModel);
        }
    }
}
