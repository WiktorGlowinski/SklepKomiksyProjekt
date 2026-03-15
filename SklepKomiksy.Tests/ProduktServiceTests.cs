using SklepKomiksy.Data;
using SklepKomiksy.Models;
using SklepKomiksy.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ComicStore.Tests
{
    public class ProduktServiceTests
    {
        // Metoda pomocnicza tworząca świeżą bazę danych w pamięci RAM dla każdego testu
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unikalna nazwa bazy dla izolacji testów
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task DodajProduktAsync_DodajeKomiksDoBazy()
        {
            // Arrange - Przygotowanie danych
            var context = GetInMemoryDbContext();
            var service = new ProduktService(context);
            var nowyKomiks = new Produkt
            {
                Tytul = "Batman: Zabójczy Żart",
                Cena = 50.00m,
                Ilosc = 5
            };

            // Act - Wykonanie akcji
            await service.DodajProduktAsync(nowyKomiks);

            // Assert - Sprawdzenie wyniku
            var komiksWBazie = await context.Produkty.FirstOrDefaultAsync(p => p.Tytul == "Batman: Zabójczy Żart");
            Assert.NotNull(komiksWBazie);
            Assert.Equal(50.00m, komiksWBazie.Cena);
            Assert.Equal(5, komiksWBazie.Ilosc);
        }

        [Fact]
        public async Task PobierzWszystkieProduktyAsync_ZwracaPoprawnaListe()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Produkty.Add(new Produkt { Tytul = "Spiderman", Cena = 30.00m, Ilosc = 2 });
            context.Produkty.Add(new Produkt { Tytul = "X-Men", Cena = 40.00m, Ilosc = 3 });
            await context.SaveChangesAsync();

            var service = new ProduktService(context);

            // Act
            var wynik = await service.PobierzWszystkieProduktyAsync();

            // Assert
            Assert.NotNull(wynik);
            Assert.Equal(2, wynik.Count);
            Assert.Contains(wynik, p => p.Tytul == "Spiderman");
            Assert.Contains(wynik, p => p.Tytul == "X-Men");
        }

        [Fact]
        public async Task UsunProduktAsync_UsuwaKomiksZBazy()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var komiks = new Produkt { Id = 1, Tytul = "Deadpool", Cena = 45.00m, Ilosc = 10 };
            context.Produkty.Add(komiks);
            await context.SaveChangesAsync();

            var service = new ProduktService(context);

            // Upewniamy się, że komiks jest w bazie
            Assert.Equal(1, await context.Produkty.CountAsync());

            // Act
            await service.UsunProduktAsync(1);

            // Assert
            var pozostale = await context.Produkty.ToListAsync();
            Assert.Empty(pozostale);
        }
    }
}
