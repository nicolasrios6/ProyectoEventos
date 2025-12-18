using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProyectoEventos.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }  
        public ICollection<Evento>? Eventos { get; set; }
    }

    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Debes ingresar un Nombre.")]
        [StringLength(50)]
        public string Nombre { get; set; }
        
        [EmailAddress]
        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria.")]
        [Compare("Clave", ErrorMessage = "Las claves no coinciden.")]
        public string Clave { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmarClave { get; set; }
    }

    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "Ingresa un email válido.")]
        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }
        public bool Recordarme { get; set; }
    }
}
