using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Biblioteca.Data;

// Administrador y Bibliotecario: CRUD completo
// Usuario: solo Index (catálogo) y Details — las demás acciones están bloqueadas por rol
[Authorize(Roles = "Administrador,Bibliotecario,Usuario")]
public class LibrosController : Controller
{
    private readonly AppDBContext _context;

    public LibrosController(AppDBContext context)
    {
        _context = context;
    }

    // GET: Libros — accesible para todos los roles
    public async Task<IActionResult> Index()
    {
        return View(await _context.Libro.ToListAsync());
    }

    // GET: Libros/Details/5 — accesible para todos los roles
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var libros = await _context.Libro.FirstOrDefaultAsync(m => m.id == id);
        if (libros == null) return NotFound();

        return View(libros);
    }

    // GET: Libros/Create — solo Administrador y Bibliotecario
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Libros/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> Create([Bind("id,nombre,autor,descripcion,catalogo,estado")] Libros libros)
    {
        if (ModelState.IsValid)
        {
            _context.Add(libros);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(libros);
    }

    // GET: Libros/Edit/5 — solo Administrador y Bibliotecario
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var libros = await _context.Libro.FindAsync(id);
        if (libros == null) return NotFound();

        return View(libros);
    }

    // POST: Libros/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> Edit(int? id, [Bind("id,nombre,autor,descripcion,catalogo,estado")] Libros libros)
    {
        if (id != libros.id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(libros);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibrosExists(libros.id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(libros);
    }

    // GET: Libros/Delete/5 — solo Administrador y Bibliotecario
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var libros = await _context.Libro.FirstOrDefaultAsync(m => m.id == id);
        if (libros == null) return NotFound();

        return View(libros);
    }

    // POST: Libros/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var libros = await _context.Libro.FindAsync(id);
        if (libros != null)
        {
            _context.Libro.Remove(libros);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LibrosExists(int? id)
    {
        return _context.Libro.Any(e => e.id == id);
    }
}
