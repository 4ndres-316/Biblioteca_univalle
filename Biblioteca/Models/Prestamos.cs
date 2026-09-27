using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Prestamos
    {
        public int id { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Usuario")]
        public int usuarioId { get; set; }

        [Required(ErrorMessage = "El libro es obligatoria")]
        [Display(Name = "Libro")]
        public int libroId { get; set; }

        [Required(ErrorMessage = "La fecha del préstamo es obligatoria")]
        [DataType(DataType.Date, ErrorMessage = "La fecha no es válida")]
        [Display(Name = "Fecha del préstamo")]
        public DateTime fechaPrestamo { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La fecha de la devolución es obligatoria")]
        [DataType(DataType.Date, ErrorMessage = "La fecha no es válida")]
        [Display(Name = "Fecha de la devolución")]
        public DateTime fechaDevolucion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public bool estado { get; set; } = true;

        //relaciones en teoria
        [ForeignKey("UsuariosId")]
        public Usuarios? usuario { get; set; }

        [ForeignKey("LibrosId")]
        public Libros? libro { get; set; }
    }
}
