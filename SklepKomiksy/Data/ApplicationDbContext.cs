using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SklepKomiksy.Models;

namespace SklepKomiksy.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Produkt> Produkty { get; set; }
        public DbSet<Autor> Autorzy { get; set; }
        public DbSet<Gatunek> Gatunki { get; set; }
        public DbSet<KategoriaWiekowa> KategorieWiekowe { get; set; }
        public DbSet<Lokalizacja> Lokalizacje { get; set; }
        public DbSet<Wydawnictwo> Wydawnictwa { get; set; }
    }
}