using SklepKomiksy.Data;
using SklepKomiksy.Models;
using SklepKomiksy.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SklepKomiksy.Components;
using SklepKomiksy.Components.Account;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Konfiguracja bazy danych MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAntiforgery();

// Nowy system uwierzytelniania w .NET 10
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

// Rejestracja Identity z dodaniem ról
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// Nowy format rejestracji Blazora
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Nasz serwis
builder.Services.AddScoped<ProduktService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery(); // Wymagane w nowym Blazorze
app.UseAuthentication();
app.UseAuthorization();
// Nowy sposób mapowania aplikacji Blazor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

// Uruchomienie Seedera
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbSeeder.SeedRolesAndAdminAsync(services);
}

app.MapPost("/api/zaloguj", async (HttpContext context, SignInManager<SklepKomiksy.Data.ApplicationUser> signInManager) =>
{
    var email = context.Request.Form["email"];
    var haslo = context.Request.Form["password"];

    var result = await signInManager.PasswordSignInAsync(email, haslo, isPersistent: false, lockoutOnFailure: false);

    return result.Succeeded ? Results.Redirect("/") : Results.Redirect("/login?error=true");
}).DisableAntiforgery();

app.MapGet("/api/wyloguj", async (Microsoft.AspNetCore.Identity.SignInManager<SklepKomiksy.Data.ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Microsoft.AspNetCore.Http.Results.Redirect("/");
});

app.Run();
