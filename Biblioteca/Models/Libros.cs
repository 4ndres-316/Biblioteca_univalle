using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Libros
    {
        public int id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        [Display(Name = "Nombre del Libro")]
        public string nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(50, ErrorMessage = "El autor no puede tener más de 50 caracteres")]
        [Display(Name = "Autor")]
        public string autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres")]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El catálogo es obligatorio")]
        [StringLength(20, ErrorMessage = "El catálogo no puede tener más de 20 caracteres")]
        [Display(Name = "Catálogo")]
        public string catalogo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public bool estado { get; set; } = true;
    }
}
