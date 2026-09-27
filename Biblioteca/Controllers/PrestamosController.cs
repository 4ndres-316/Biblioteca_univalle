using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Biblioteca.Data;

// Todos los roles autenticados acceden, pero Usuario solo ve sus propios préstamos
[Authorize(Roles = "Administrador,Bibliotecario,Usuario")]
public class PrestamosController : Controller
{
    private readonly AppDBContext _context;

    public PrestamosController(AppDBContext context)
    {
        _context = context;
    }

    // GET: Prestamos
    // Administrador y Bibliotecario ven todos; Usuario solo los suyos
    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Usuario"))
        {
            // El Usuario solo ve préstamos asociados a su nombre de usuario
            var userName = User.Identity!.Name;
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.usuario == userName);
            if (usuario == null) return View(new List<Prestamos>());

            var misPrestamos = await _context.Prestamo
                .Include(p => p.usuario)
                .Include(p => p.libro)
                .Where(p => p.usuarioId == usuario.id)
                .ToListAsync();
            return View(misPrestamos);
        }

        // Administrador y Bibliotecario: todos los préstamos
        var prestamos = await _context.Prestamo
            .Include(p => p.usuario)
            .Include(p => p.libro)
            .ToListAsync();
        return View(prestamos);
    }

    // GET: Prestamos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var prestamos = await _context.Prestamo
            .Include(p => p.usuario)
            .Include(p => p.libro)
            .FirstOrDefaultAsync(m => m.id == id);
        if (prestamos == null) return NotFound();

        // Usuario solo puede ver sus propios préstamos
        if (User.IsInRole("Usuario"))
        {
            var userName = User.Identity!.Name;
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.usuario == userName);
            if (usuario == null || prestamos.usuarioId != usuario.id)
                return Forbid();
        }

        return View(prestamos);
    }

    // GET: Prestamos/Create — solo Administrador y Bibliotecario
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public IActionResult Create()
    {
        ViewBag.Usuarios = _context.Usuario.Where(u => u.estado).ToList();
        ViewBag.Libros   = _context.Libro.Where(l => l.estado).ToList();
        return View();
    }

    // POST: Prestamos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> Create([Bind("id,usuarioId,libroId,fechaPrestamo,fechaDevolucion,estado")] Prestamos prestamos)
    {
        if (ModelState.IsValid)
        {
            _context.Add(prestamos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Usuarios = _context.Usuario.Where(u => u.estado).ToList();
        ViewBag.Libros   = _context.Libro.Where(l => l.estado).ToList();
        return View(prestamos);
    }

    // GET: Prestamos/Edit/5 — solo Administrador y Bibliotecario
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var prestamos = await _context.Prestamo.FindAsync(id);
        if (prestamos == null) return NotFound();

        ViewBag.Usuarios = _context.Usuario.Where(u => u.estado).ToList();
        ViewBag.Libros   = _context.Libro.Where(l => l.estado).ToList();
        return View(prestamos);
    }

    // POST: Prestamos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> Edit(int? id, [Bind("id,usuarioId,libroId,fechaPrestamo,fechaDevolucion,estado")] Prestamos prestamos)
    {
        if (id != prestamos.id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(prestamos);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamosExists(prestamos.id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Usuarios = _context.Usuario.Where(u => u.estado).ToList();
        ViewBag.Libros   = _context.Libro.Where(l => l.estado).ToList();
        return View(prestamos);
    }

    // GET: Prestamos/Delete/5 — solo Administrador y Bibliotecario
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var prestamos = await _context.Prestamo
            .Include(p => p.usuario)
            .Include(p => p.libro)
            .FirstOrDefaultAsync(m => m.id == id);
        if (prestamos == null) return NotFound();

        return View(prestamos);
    }

    // POST: Prestamos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var prestamos = await _context.Prestamo.FindAsync(id);
        if (prestamos != null)
        {
            _context.Prestamo.Remove(prestamos);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PrestamosExists(int? id)
    {
        return _context.Prestamo.Any(e => e.id == id);
    }
}
