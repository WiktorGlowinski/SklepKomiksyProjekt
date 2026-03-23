using Microsoft.EntityFrameworkCore;
using SklepKomiksy.Data;
using SklepKomiksy.Models;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace SklepKomiksy.Services
{
    public class ProduktService
    {
        private readonly ApplicationDbContext _context;

        public ProduktService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Produkt>> PobierzWszystkieProduktyAsync() => await _context.Produkty.ToListAsync();

        public async Task<Produkt?> PobierzProduktPoIdAsync(long id) => await _context.Produkty.FindAsync(id);

        public async Task DodajProduktAsync(Produkt produkt)
        {
            _context.Produkty.Add(produkt);
            await _context.SaveChangesAsync();
        }

        public async Task AktualizujProduktAsync(Produkt produkt)
        {
            _context.Produkty.Update(produkt);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Lokalizacja>> PobierzLokalizacjeAsync()
        {
            return await _context.Lokalizacje.ToListAsync();
        }

        public async Task<List<KategoriaWiekowa>> PobierzKategorieWiekoweAsync()
        {
            return await _context.KategorieWiekowe.ToListAsync();
        }

        public async Task UsunProduktAsync(long id)
        {
            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt != null)
            {
                _context.Produkty.Remove(produkt);
                await _context.SaveChangesAsync();
            }
        }

        // Słowniki
        public async Task<List<Autor>> PobierzAutorowAsync() => await _context.Autorzy.ToListAsync();
        public async Task<List<Wydawnictwo>> PobierzWydawnictwaAsync() => await _context.Wydawnictwa.ToListAsync();
        public async Task<List<Gatunek>> PobierzGatunkiAsync() => await _context.Gatunki.ToListAsync();
       

        public async Task DodajAutoraAsync(Autor autor)
        {
            _context.Autorzy.Add(autor);
            await _context.SaveChangesAsync();
        }

        public async Task DodajLokalizacjeAsync(Lokalizacja lokalizacja)
        {
            _context.Lokalizacje.Add(lokalizacja);
            await _context.SaveChangesAsync();
        }
// --- Metody dla Gatunków ---
        public async Task DodajGatunekAsync(Gatunek gatunek)
        {
            _context.Gatunki.Add(gatunek);
            await _context.SaveChangesAsync();
        }
        public async Task InicjalizujKategorieWiekoweAsync()
        {
            // Jeśli tabela kategorii w bazie jest pusta, system sam doda te 4 podstawowe opcje
            if (!_context.KategorieWiekowe.Any())
            {
                _context.KategorieWiekowe.AddRange(
                    new KategoriaWiekowa { Nazwa = "familijne" },
                    new KategoriaWiekowa { Nazwa = "12+" },
                    new KategoriaWiekowa { Nazwa = "16+" },
                    new KategoriaWiekowa { Nazwa = "18+" }
                );
                await _context.SaveChangesAsync();
            }
        }
        public async Task UsunGatunekAsync(long id)
        {
            var gatunek = await _context.Gatunki.FindAsync(id);
            if (gatunek != null)
            {
                _context.Gatunki.Remove(gatunek);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DodajKomiksAsync(Produkt komiks, long[] wybraneAutorzyId, long[] wybraneGatunkiId)
        {
            // 1. Zabezpieczenie przed błędem ReadOnlySpan w .NET 8 - zamieniamy tablice na proste listy
            var listaAutorowId = wybraneAutorzyId.ToList();
            var listaGatunkowId = wybraneGatunkiId.ToList();

            // 2. Szukamy w bazie za pomocą zwykłych list
            var autorzyDB = await _context.Autorzy
                .Where(a => listaAutorowId.Contains(a.Id))
                .ToListAsync();

            var gatunkiDB = await _context.Gatunki
                .Where(g => listaGatunkowId.Contains(g.Id))
                .ToListAsync();

            // 3. Przypinamy relacje do komiksu
            komiks.Autorzy = autorzyDB;
            komiks.Gatunki = gatunkiDB;

            // 4. Zapisujemy w bazie
            _context.Produkty.Add(komiks);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Produkt>> PobierzKomiksyAsync() // <-- użyj swojej nazwy metody
        {
            return await _context.Produkty
                .Include(p => p.Autorzy)
                .Include(p => p.Gatunki)
                .ToListAsync();
        }

        // 1. Zmiana ilości sztuk (+ / -)
        public async Task ZmienIloscAsync(long id, int zmiana, string uzytkownik)
        {
            var komiks = await _context.Produkty.FindAsync(id);
            if (komiks != null)
            {
                int staraIlosc = komiks.Ilosc;
                komiks.Ilosc += zmiana;
                if (komiks.Ilosc < 0) komiks.Ilosc = 0;

                // LOGIKA REJESTRU AKTYWNOŚCI:
                var historia = new HistoriaOperacji
                {
                    Uzytkownik = uzytkownik,
                    TypAkcji = zmiana > 0 ? "Dostawa" : "Sprzedaż / Ubytek",
                    NazwaKomiksu = komiks.Tytul,
                    Szczegoly = $"Zmieniono stan magazynowy z {staraIlosc} na {komiks.Ilosc} szt."
                };
                _context.HistorieOperacji.Add(historia);

                await _context.SaveChangesAsync();
            }
        }

        // 2. Szybka zmiana lokalizacji z poziomu tabeli
        public async Task ZmienLokalizacjeAsync(long id, int? noweIdLokalizacji)
        {
            var komiks = await _context.Produkty.FindAsync(id);
            if (komiks != null)
            {
                komiks.IdLokalizacji = noweIdLokalizacji;
                await _context.SaveChangesAsync();
            }
        }

        // 3. Usuwanie komiksu (tylko dla Szefa)
        public async Task UsunKomiksAsync(long id, string uzytkownik)
        {
            var komiks = await _context.Produkty.FindAsync(id);
            if (komiks != null)
            {
                // LOGIKA REJESTRU AKTYWNOŚCI:
                var historia = new HistoriaOperacji
                {
                    Uzytkownik = uzytkownik,
                    TypAkcji = "Usunięcie",
                    NazwaKomiksu = komiks.Tytul,
                    Szczegoly = "Całkowicie usunięto pozycję z bazy danych."
                };
                _context.HistorieOperacji.Add(historia);

                _context.Produkty.Remove(komiks);
                await _context.SaveChangesAsync();
            }
        }
        // --- Metody dla Wydawnictw ---

        public async Task DodajWydawnictwoAsync(Wydawnictwo wydawnictwo)
        {
            _context.Wydawnictwa.Add(wydawnictwo);
            await _context.SaveChangesAsync();
        }

        public async Task UsunWydawnictwoAsync(long id)
        {
            var wydawnictwo = await _context.Wydawnictwa.FindAsync(id);
            if (wydawnictwo != null)
            {
                _context.Wydawnictwa.Remove(wydawnictwo);
                await _context.SaveChangesAsync();
            }

        }
    }
}
