using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Usuarios
    {
        public int id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [StringLength(30, ErrorMessage = "El correo no puede tener más de 30 caracteres")]
        [EmailAddress(ErrorMessage = "El correo no tiene el formato válido")]
        [Display(Name = "Correo")]
        public string correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(30, ErrorMessage = "El usuario no puede tener más de 30 caracteres")]
        [Display(Name = "Usuario")]
        public string usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatorio")]
        [StringLength(10, ErrorMessage = "La contraseña no puede tener más de 10 caracteres")]
        [Display(Name = "Contraseña")]
        public string contrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio")]
        [StringLength(15, ErrorMessage = "El rol no puede tener más de 15 caracteres")]
        [Display(Name = "Rol Usuario")]
        public string rol { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public bool estado { get; set; } = true;
    }
}
