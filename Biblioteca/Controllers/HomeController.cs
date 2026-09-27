using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

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

        // Redirige al dashboard según el rol guardado en el Claim de la cookie
        public IActionResult Index()
        {
            if (User.IsInRole("Administrador"))
                return RedirectToAction(nameof(DashboardAdmin));

            if (User.IsInRole("Bibliotecario"))
                return RedirectToAction(nameof(DashboardBibliotecario));

            if (User.IsInRole("Usuario"))
                return RedirectToAction(nameof(DashboardUsuario));

            return View();
        }

        // ── Dashboard Administrador ───────────────────────────────────────────
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DashboardAdmin()
        {
            ViewBag.TotalLibros      = await _context.Libro.CountAsync();
            ViewBag.LibrosActivos    = await _context.Libro.CountAsync(l => l.estado);
            ViewBag.TotalUsuarios    = await _context.Usuario.CountAsync();
            ViewBag.UsuariosActivos  = await _context.Usuario.CountAsync(u => u.estado);
            ViewBag.TotalPrestamos   = await _context.Prestamo.CountAsync();
            ViewBag.PrestamosActivos = await _context.Prestamo.CountAsync(p => p.estado);
            return View();
        }

        // ── Dashboard Bibliotecario ───────────────────────────────────────────
        [Authorize(Roles = "Bibliotecario")]
        public async Task<IActionResult> DashboardBibliotecario()
        {
            ViewBag.TotalLibros      = await _context.Libro.CountAsync();
            ViewBag.LibrosActivos    = await _context.Libro.CountAsync(l => l.estado);
            ViewBag.TotalPrestamos   = await _context.Prestamo.CountAsync();
            ViewBag.PrestamosActivos = await _context.Prestamo.CountAsync(p => p.estado);
            return View();
        }

        // ── Dashboard Usuario ─────────────────────────────────────────────────
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> DashboardUsuario()
        {
            // Leer el id del usuario desde el Claim (lo pusimos al hacer login)
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var usuarioId = int.TryParse(idClaim, out var id) ? id : 0;

            ViewBag.LibrosDisponibles   = await _context.Libro.CountAsync(l => l.estado);
            ViewBag.MisPrestamos        = await _context.Prestamo.CountAsync(p => p.usuarioId == usuarioId);
            ViewBag.MisPrestamosActivos = await _context.Prestamo.CountAsync(p => p.usuarioId == usuarioId && p.estado);
            ViewBag.NombreUsuario       = User.FindFirstValue(ClaimTypes.GivenName) ?? User.Identity!.Name;

            return View();
        }

        // ── Error ─────────────────────────────────────────────────────────────
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
