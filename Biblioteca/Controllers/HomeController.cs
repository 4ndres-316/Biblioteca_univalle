using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;

        public HomeController(AppDBContext context)
        {
            _context = context;
        }

        // Redirige al dashboard según el rol
        public IActionResult Index()
        {
            if (User.IsInRole("Administrador"))
                return RedirectToAction(nameof(DashboardAdmin));
            if (User.IsInRole("Bibliotecario"))
                return RedirectToAction(nameof(DashboardBibliotecario));
            if (User.IsInRole("Usuario"))
                return RedirectToAction(nameof(DashboardUsuario));

            // Si no tiene rol asignado aún, muestra página genérica
            return View();
        }

        // Dashboard Administrador: resumen de libros, usuarios y préstamos
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DashboardAdmin()
        {
            ViewBag.TotalLibros     = await _context.Libro.CountAsync();
            ViewBag.LibrosActivos   = await _context.Libro.CountAsync(l => l.estado);
            ViewBag.TotalUsuarios   = await _context.Usuario.CountAsync();
            ViewBag.UsuariosActivos = await _context.Usuario.CountAsync(u => u.estado);
            ViewBag.TotalPrestamos  = await _context.Prestamo.CountAsync();
            ViewBag.PrestamosActivos = await _context.Prestamo.CountAsync(p => p.estado);
            return View();
        }

        // Dashboard Bibliotecario: resumen de libros y préstamos
        [Authorize(Roles = "Bibliotecario")]
        public async Task<IActionResult> DashboardBibliotecario()
        {
            ViewBag.TotalLibros      = await _context.Libro.CountAsync();
            ViewBag.LibrosActivos    = await _context.Libro.CountAsync(l => l.estado);
            ViewBag.TotalPrestamos   = await _context.Prestamo.CountAsync();
            ViewBag.PrestamosActivos = await _context.Prestamo.CountAsync(p => p.estado);
            return View();
        }

        // Dashboard Usuario: sus préstamos activos y total de libros disponibles
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> DashboardUsuario()
        {
            var userName = User.Identity!.Name;
            var usuario  = await _context.Usuario.FirstOrDefaultAsync(u => u.usuario == userName);

            ViewBag.LibrosDisponibles = await _context.Libro.CountAsync(l => l.estado);

            if (usuario != null)
            {
                ViewBag.MisPrestamos       = await _context.Prestamo.CountAsync(p => p.usuarioId == usuario.id);
                ViewBag.MisPrestamosActivos = await _context.Prestamo.CountAsync(p => p.usuarioId == usuario.id && p.estado);
                ViewBag.NombreUsuario      = usuario.nombre;
            }
            else
            {
                ViewBag.MisPrestamos        = 0;
                ViewBag.MisPrestamosActivos = 0;
                ViewBag.NombreUsuario       = userName;
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
