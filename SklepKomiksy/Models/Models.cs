using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SklepKomiksy.Models
{
    [Table("autorzy")]
    public class Autor
    {
        [Key][Column("id_autora")] public long Id { get; set; }
        [Column("imie")] public string Imie { get; set; } = string.Empty;
        [Column("nazwisko")] public string Nazwisko { get; set; } = string.Empty;
    }

    [Table("gatunki")]
    public class Gatunek
    {
        [Key][Column("id_gatunku")] public long Id { get; set; }
        [Column("gatunek")] public string Nazwa { get; set; } = string.Empty;
    }

    [Table("kategorie_wiekowe")]
    public class KategoriaWiekowa
    {
        [Key][Column("id_kategorii")] public long Id { get; set; }
        [Column("18+")] public bool? OsiemnasciePlus { get; set; }
    }

    [Table("lokalizacje")]
    public class Lokalizacja
    {
        [Key][Column("id_lokalizacji")] public long Id { get; set; }
        [Column("regal")] public string? Regal { get; set; }
        [Column("polka")] public int? Polka { get; set; }
    }

    [Table("wydawnictwa")]
    public class Wydawnictwo
    {
        [Key][Column("id_wydawnictwa")] public long Id { get; set; }
        [Column("wydawnictwo")] public string Nazwa { get; set; } = string.Empty;
    }

    [Table("produkty")]
    public class Produkt
    {
        [Key][Column("id_produktu")] public long Id { get; set; }
        [Column("tytul")] public string Tytul { get; set; } = string.Empty;
        [Column("numer_zeszytu")] public int? NumerZeszytu { get; set; }
        [Column("isbn")] public string? ISBN { get; set; }
        [Column("cena")] public decimal Cena { get; set; }
        [Column("ilosc")] public int Ilosc { get; set; }
        [Column("id_wydawnictwa")] public int? IdWydawnictwa { get; set; }
        [Column("id_kategorii_wiekowej")] public int? IdKategoriiWiekowej { get; set; }
        [Column("id_lokalizacji")] public int? IdLokalizacji { get; set; }
        [Column("id_autora")] public int? IdAutora { get; set; }
        [Column("id_gatunku")] public int? IdGatunku { get; set; }
    }
}
