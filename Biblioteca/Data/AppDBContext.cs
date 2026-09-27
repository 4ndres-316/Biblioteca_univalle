using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;

namespace Biblioteca.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options) { }

        public DbSet<Usuarios> Usuario { get; set; }
        public DbSet<Libros> Libro { get; set; }
        public DbSet<Prestamos> Prestamo { get; set; }
    }
}
