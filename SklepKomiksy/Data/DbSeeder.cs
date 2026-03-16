using Microsoft.AspNetCore.Identity;

namespace SklepKomiksy.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Wlasciciel", "Pracownik" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminEmail = "admin@sklep.pl";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userManager.CreateAsync(adminUser, "Szef123!");
                await userManager.AddToRoleAsync(adminUser, "Wlasciciel");
            }
            var pracownikEmail = "pracownik@sklep.pl";
            var pracownikUser = await userManager.FindByEmailAsync(pracownikEmail);

            if (pracownikUser == null)
            {
                pracownikUser = new ApplicationUser { UserName = pracownikEmail, Email = pracownikEmail, EmailConfirmed = true };
                // Uwaga: Hasło musi mieć dużą literę, cyfrę i znak specjalny
                await userManager.CreateAsync(pracownikUser, "Pracownik123!");
                await userManager.AddToRoleAsync(pracownikUser, "Pracownik");
            }
        }
    }
}