using Microsoft.EntityFrameworkCore;
using SklepKomiksy.Data;
using SklepKomiksy.Models;
using SklepKomiksy.Services;
using Xunit;

namespace SklepKomiksy.Tests
{
    public class ProduktServiceTests
    {
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        [Fact]
        public async Task DodajKomiksAsync_PowinienZapisacKomiksZOkladkaIHistora()
        {
            // Arrange
            var context = GetDatabaseContext();
            var service = new ProduktService(context);
            var nowyKomiks = new Produkt
            {
                Tytul = "Batman: Year One",
                Cena = 89.99m,
                Ilosc = 10,
                
            };

            // Act
            await service.DodajKomiksAsync(nowyKomiks, new long[] { }, new long[] { });

            // Assert
            var komiksWBazie = await context.Produkty.FirstOrDefaultAsync(p => p.Tytul == "Batman: Year One");
            Assert.NotNull(komiksWBazie);
          

            // Sprawdzenie czy Seeder historii zadziałał
            var log = await context.HistorieOperacji.FirstOrDefaultAsync();
            Assert.NotNull(log);
            Assert.Equal("Batman: Year One", log.NazwaKomiksu);
        }

        [Fact]
        public async Task PobierzStatystyki_PowinienZwracacPoprawneWartosciDlaDashboardu()
        {
            // Arrange
            var context = GetDatabaseContext();
            var service = new ProduktService(context);

            context.Produkty.AddRange(new List<Produkt>
            {
                new Produkt { Tytul = "A", Cena = 100, Ilosc = 2 }, // Wartość 200
                new Produkt { Tytul = "B", Cena = 50, Ilosc = 0 }   // Brak w magazynie
            });
            await context.SaveChangesAsync();

            // Act
            var komiksy = await service.PobierzKomiksyAsync();
            var wartosc = komiksy.Sum(k => k.Cena * k.Ilosc);
            var braki = komiksy.Count(k => k.Ilosc == 0);

            // Assert
            Assert.Equal(200, wartosc);
            Assert.Equal(1, braki);
        }

        [Fact]
        public async Task UsunKomiksAsync_PowinienUsunacProduktIZapisacLog()
        {
            // Arrange
            var context = GetDatabaseContext();
            var service = new ProduktService(context);
            var komiks = new Produkt { Tytul = "Do Usuniecia", Cena = 10, Ilosc = 1 };
            context.Produkty.Add(komiks);
            await context.SaveChangesAsync();

            // Act
            await service.UsunKomiksAsync(komiks.Id, "Testowy admin");

            // Assert
            var czyIstnieje = await context.Produkty.AnyAsync(p => p.Id == komiks.Id);
            var log = await context.HistorieOperacji.OrderByDescending(h => h.DataCzas).FirstOrDefaultAsync();

            Assert.False(czyIstnieje);
            Assert.Equal("Usunięcie", log?.TypAkcji);
            Assert.Equal("Do Usuniecia", log?.NazwaKomiksu);
        }
    }
}