using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using SistemasAnaliticosWEB.Helpers;
using SistemasAnaliticosWEB.Models;
using System.Net;
using System.Net.Mail;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;


namespace SistemasAnaliticosWEB.Controllers
{
    public class ContactoController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ContactoController> _logger;

        public ContactoController(IConfiguration config, ILogger<ContactoController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [EnableRateLimiting("default")]
        public ActionResult Index()
        {
            return View();
        }

        [EnableRateLimiting("default")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EnviarCorreo(string nombre, string email, string telefono, string mensaje)
        {
            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(mensaje))
            {
                return Json(new { success = false, error = "Campos requeridos incompletos." });
            }

            try
            {
                var tenantId = _config["SmtpSettings:TenantId"];
                var clientId = _config["SmtpSettings:ClientId"];
                var clientSecret = _config["SmtpSettings:ClientSecret"];
                var fromEmail = _config["SmtpSettings:FromEmail"];
                var toEmail = _config["SmtpSettings:ToEmail"];

                if (string.IsNullOrWhiteSpace(clientSecret))
                    return Json(new { success = false, error = "Configuración no encontrada." });

                var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                var graphClient = new GraphServiceClient(credential);

                var message = new Message
                {
                    Subject = "Nuevo mensaje desde el sitio web",
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = $"""
                        <!DOCTYPE html>
                        <html>
                        <body style='margin: 0; padding: 0; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif; background-color: #e8e8e8;'>
                            <table role='presentation' cellpadding='0' cellspacing='0' border='0' width='100%' style='background-color: #e8e8e8;'>
                                <tr>
                                    <td align='center' style='padding: 40px 20px;'>
                                        <table role='presentation' cellpadding='0' cellspacing='0' border='0' width='600' style='background-color: #ffffff; max-width: 600px;'>
                                            <tr>
                                                <td style='padding: 40px 40px 30px 40px; border-top: 4px solid #B7041A;'>
                                                    <h2 style='margin: 0; color: #333333;'>Nuevo mensaje de contacto</h2>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style='padding: 0 40px 10px 40px;'>
                                                    <table role='presentation' cellpadding='0' cellspacing='0' border='0' width='100%' style='margin-bottom: 20px;'>
                                                        <tr>
                                                            <td style='padding: 12px 0; border-bottom: 1px solid #e0e0e0;'>
                                                                <strong style='color: #B7041A;'>Nombre:</strong>
                                                                <span style='color: #555555; margin-left: 10px;'>{nombre}</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style='padding: 12px 0; border-bottom: 1px solid #e0e0e0;'>
                                                                <strong style='color: #B7041A;'>Correo:</strong>
                                                                <span style='color: #555555; margin-left: 10px;'>{email}</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style='padding: 12px 0; border-bottom: 1px solid #e0e0e0;'>
                                                                <strong style='color: #B7041A;'>Teléfono:</strong>
                                                                <span style='color: #555555; margin-left: 10px;'>{telefono}</span>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <h3 style='margin: 20px 0 10px 0; color: #333333; font-size: 18px;'>Mensaje:</h3>
                                                    <p style='margin: 0 0 20px 0; font-size: 16px; line-height: 1.6; color: #555555; background-color: #f9f9f9; padding: 15px; border-radius: 8px;'>
                                                        {mensaje}
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style='padding: 30px 40px 20px 40px;'>
                                                    <p style='margin: 0 0 10px 0; font-size: 16px; line-height: 1.5; color: #555555;'>
                                                        Mensaje enviado a través de la página web del formulario de contacto.
                                                    </p>
                                                    <p style='margin: 0; font-size: 16px; line-height: 1.5; color: #555555;'>
                                                        Sistemas Analíticos S.A
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style='padding-bottom: 10px; border-bottom: 4px solid #B7041A;'></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </body>
                        </html>
                        """
                    },
                    ToRecipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = toEmail
                            }
                        }
                    }
                };

                await graphClient
                    .Users[fromEmail]
                    .SendMail
                    .PostAsync(new SendMailPostRequestBody
                    {
                        Message = message,
                        SaveToSentItems = true
                    });

                return Json(new { success = true });
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Graph API error al enviar correo desde {Email}", email);
                return Json(new { success = false, error = "Error al enviar. Intente más tarde." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al enviar correo desde {Email}", email);
                return Json(new { success = false, error = "Error al enviar. Intente más tarde." });
            }
        }
    }
}
