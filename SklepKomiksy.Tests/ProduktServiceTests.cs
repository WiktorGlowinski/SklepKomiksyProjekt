using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SklepKomiksy.Data;
using SklepKomiksy.Models;
using SklepKomiksy.Services;
using Xunit;

namespace SklepKomiksy.Tests
{
    public class ProduktServiceTests
    {
        // Narzędzie pomocnicze: tworzy świeżą, pustą bazę danych w pamięci RAM dla każdego testu
        private async Task<ApplicationDbContext> PrzygotujBazeWymyslona()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task ZmienIloscAsync_PoprawnieZmniejszaIlosc_OrazZapisujeAkcjeWRejestrze()
        {
            // Arrange (Przygotowanie środowiska)
            var context = await PrzygotujBazeWymyslona();
            var nowyKomiks = new Produkt { Id = 1, Tytul = "Batman: Zabójczy Żart", Ilosc = 5, Cena = 100m };
            context.Produkty.Add(nowyKomiks);
            await context.SaveChangesAsync();

            var serwis = new ProduktService(context);
            string testowyPracownik = "jan.kowalski";

            // Act (Wykonanie akcji)
            // Zmniejszamy ilość o 1, udając, że klika to Jan Kowalski
            await serwis.ZmienIloscAsync(1, -1, testowyPracownik);

            // Assert (Sprawdzenie wyników)
            var komiksPoZmianie = await context.Produkty.FindAsync(1L);
            Assert.NotNull(komiksPoZmianie);
            Assert.Equal(4, komiksPoZmianie.Ilosc); // Sprawdzamy, czy ubyła sztuka

            // Sprawdzamy nową funkcję: czy utworzył się wpis w historii!
            var historia = await context.HistorieOperacji.FirstOrDefaultAsync();
            Assert.NotNull(historia);
            Assert.Equal(testowyPracownik, historia.Uzytkownik);
            Assert.Equal("Sprzedaż / Ubytek", historia.TypAkcji);
            Assert.Equal("Batman: Zabójczy Żart", historia.NazwaKomiksu);
            Assert.Contains("z 5 na 4", historia.Szczegoly);
        }

        [Fact]
        public async Task ZmienIloscAsync_NiePozwalaNaZejsciePonizejZera()
        {
            // Arrange
            var context = await PrzygotujBazeWymyslona();
            context.Produkty.Add(new Produkt { Id = 2, Tytul = "Spider-Man", Ilosc = 0, Cena = 50m });
            await context.SaveChangesAsync();

            var serwis = new ProduktService(context);

            // Act
            // Próbujemy sprzedać komiks, którego już nie ma
            await serwis.ZmienIloscAsync(2, -1, "admin");

            // Assert
            var komiksPoZmianie = await context.Produkty.FindAsync(2L);
            Assert.Equal(0, komiksPoZmianie.Ilosc); // Ilosc nadal powinna wynosić 0, nie -1!
        }

        [Fact]
        public async Task UsunKomiksAsync_UsuwaPozycjeZBazy_OrazLogujeAkcje()
        {
            // Arrange
            var context = await PrzygotujBazeWymyslona();
            context.Produkty.Add(new Produkt { Id = 3, Tytul = "The Boys", Ilosc = 10, Cena = 150m });
            await context.SaveChangesAsync();

            var serwis = new ProduktService(context);
            string testowySzef = "szef.wlasciciel";

            // Act
            await serwis.UsunKomiksAsync(3, testowySzef);

            // Assert
            var sprawdzKomiks = await context.Produkty.FindAsync(3L);
            Assert.Null(sprawdzKomiks); // Komiksu już nie ma

            // Sprawdzamy rejestr operacji
            var historia = await context.HistorieOperacji.FirstOrDefaultAsync();
            Assert.NotNull(historia);
            Assert.Equal(testowySzef, historia.Uzytkownik);
            Assert.Equal("Usunięcie", historia.TypAkcji);
            Assert.Equal("The Boys", historia.NazwaKomiksu);
        }
    }
}
