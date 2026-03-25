using System;

namespace SklepKomiksy.Models
{
    public class HistoriaOperacji
    {
        public long Id { get; set; }
        public DateTime DataCzas { get; set; } = DateTime.Now;
        public string Uzytkownik { get; set; } = string.Empty;
        public string TypAkcji { get; set; } = string.Empty; // np. "Dodanie", "Usunięcie", "Sprzedaż"
        public string NazwaKomiksu { get; set; } = string.Empty;
        public string Szczegoly { get; set; } = string.Empty;
    }
}