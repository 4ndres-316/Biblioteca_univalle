using Biblioteca.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC + Razor Pages (necesario para las páginas de Identity)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Base de datos
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Identity con soporte de roles
// AddIdentity en lugar de AddDefaultIdentity para habilitar RoleManager
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // sin verificación de email por ahora
    options.Password.RequireDigit           = false;
    options.Password.RequireLowercase       = false;
    options.Password.RequireUppercase       = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength         = 6;
})
.AddEntityFrameworkStores<AppDBContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();  // mantiene las páginas de Razor de Identity (Login, Register, etc.)

// Redirigir a Login si no está autenticado
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath    = "/Identity/Account/Login";
    options.LogoutPath   = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();

// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// Razor Pages para Identity (Login, Register, etc.)
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Sembrar los roles al iniciar la aplicación
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    foreach (var rol in new[] { "Administrador", "Bibliotecario", "Usuario" })
    {
        if (!await roleManager.RoleExistsAsync(rol))
            await roleManager.CreateAsync(new IdentityRole(rol));
    }
}

app.Run();
