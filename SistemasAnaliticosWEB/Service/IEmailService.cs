namespace SistemasAnaliticosWEB.Service
{
    public interface IEmailService
    {
        Task SendContactEmailAsync(string nombre, string email, string telefono, string mensaje);
    }
}
