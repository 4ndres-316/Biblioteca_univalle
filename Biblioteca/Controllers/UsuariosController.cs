using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Biblioteca.Data;

// Solo el Administrador puede gestionar usuarios
[Authorize(Roles = "Administrador")]
public class UsuariosController : Controller
{
    private readonly AppDBContext _context;

    public UsuariosController(AppDBContext context)
    {
        _context = context;
    }

    // GET: Usuarios
    public async Task<IActionResult> Index()
    {
        return View(await _context.Usuario.ToListAsync());
    }

    // GET: Usuarios/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var usuarios = await _context.Usuario.FirstOrDefaultAsync(m => m.id == id);
        if (usuarios == null) return NotFound();

        return View(usuarios);
    }

    // GET: Usuarios/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Usuarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("id,nombre,correo,usuario,contrasena,rol,estado")] Usuarios usuarios)
    {
        if (ModelState.IsValid)
        {
            _context.Add(usuarios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(usuarios);
    }

    // GET: Usuarios/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var usuarios = await _context.Usuario.FindAsync(id);
        if (usuarios == null) return NotFound();

        return View(usuarios);
    }

    // POST: Usuarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("id,nombre,correo,usuario,contrasena,rol,estado")] Usuarios usuarios)
    {
        if (id != usuarios.id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(usuarios);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExists(usuarios.id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(usuarios);
    }

    // GET: Usuarios/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var usuarios = await _context.Usuario.FirstOrDefaultAsync(m => m.id == id);
        if (usuarios == null) return NotFound();

        return View(usuarios);
    }

    // POST: Usuarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var usuarios = await _context.Usuario.FindAsync(id);
        if (usuarios != null)
        {
            _context.Usuario.Remove(usuarios);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UsuariosExists(int? id)
    {
        return _context.Usuario.Any(e => e.id == id);
    }
}
