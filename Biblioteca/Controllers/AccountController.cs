using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Biblioteca.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDBContext _context;

        public AccountController(AppDBContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            // Buscar por campo "usuario" y contraseña
            var usuarioDb = await _context.Usuario
                .FirstOrDefaultAsync(u => u.usuario == model.Usuario
                                       && u.contrasena == model.Password
                                       && u.estado == true);

            if (usuarioDb == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(model);
            }

            var rol = usuarioDb.rol;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioDb.id.ToString()),
                new Claim(ClaimTypes.Name,           usuarioDb.usuario),
                new Claim(ClaimTypes.GivenName,      usuarioDb.nombre),
                new Claim(ClaimTypes.Role,           rol),
            };

            var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identidad);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = model.RememberMe });

            return rol switch
            {
                "Administrador" => RedirectToAction("DashboardAdmin",         "Home"),
                "Bibliotecario" => RedirectToAction("DashboardBibliotecario", "Home"),
                "Usuario"       => RedirectToAction("DashboardUsuario",       "Home"),
                _               => RedirectToAction("Index",                  "Home"),
            };
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/AccesoDenegado
        [AllowAnonymous]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}
