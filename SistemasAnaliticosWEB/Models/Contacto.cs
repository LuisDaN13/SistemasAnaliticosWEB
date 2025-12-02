using System.ComponentModel.DataAnnotations;

namespace SistemasAnaliticosWEB.Models
{
    public class Contacto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El mensaje es requerido")]
        [Display(Name = "Mensaje")]
        [StringLength(1000, ErrorMessage = "El mensaje no puede exceder 1000 caracteres")]
        public string Mensaje { get; set; }
    }
}
