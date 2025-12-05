using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;
using System.Net.Mail;

namespace SistemasAnaliticosWEB.Controllers
{
    public class ContactoController : Controller
    {
        [EnableRateLimiting("default")]
        public ActionResult Index()
        {
            return View();
        }

        [EnableRateLimiting("default")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EnviarCorreo(string nombre, string email, string telefono, string mensaje)
        {
            try
            {
                var correo = new MailMessage();
                correo.From = new MailAddress("no-reply@sasacr.com");
                correo.To.Add("spti@SistemasAnaliticos.Cr");

                correo.Subject = "Nuevo mensaje desde el sitio web";
                correo.Body =
                    $"Nombre: {nombre}\n" +
                    $"Correo: {email}\n" +
                    $"Teléfono: {telefono}\n\n" +
                    $"Mensaje:\n{mensaje}";
                correo.IsBodyHtml = false;

                var smtp = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                        "no-reply@sasacr.com",  // correo de 365
                        "Sasa2025*" // app password si tienes MFA
                    )
                };

                smtp.Send(correo);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
