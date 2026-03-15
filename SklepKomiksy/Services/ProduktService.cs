using ComicStore.Data;
using ComicStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicStore.Services
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
        public async Task<List<KategoriaWiekowa>> PobierzKategorieWiekoweAsync() => await _context.KategorieWiekowe.ToListAsync();
        public async Task<List<Lokalizacja>> PobierzLokalizacjeAsync() => await _context.Lokalizacje.ToListAsync();

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
    }
}
