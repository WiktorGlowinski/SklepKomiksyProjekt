# SklepKomiksyProjekt
systemy informatyczne projekt 1/2 semestr letni 2026
Pewnie, Wiktor! Dobry plik README.md to podstawa każdego projektu, zwłaszcza jeśli będziesz chciał do niego wrócić po jakimś czasie lub wrzucić go na GitHuba jako część swojego portfolio.

Przygotowałem dla Ciebie kompletny plik w formacie Markdown, który opisuje całą strukturę, użyte technologie oraz instrukcję uruchomienia aplikacji.

Wystarczy, że skopiujesz poniższy tekst i wkleisz go do pliku o nazwie README.md w głównym folderze Twojego projektu.

Markdown
# Sklep z Komiksami - System Zarządzania Magazynem 🦸‍♂️📚

Lokalna aplikacja webowa stworzona w celu zarządzania stanem magazynowym sklepu z komiksami. Pozwala na sprawdzanie dostępności produktów, dodawanie nowych komiksów, edycję istniejących oraz zarządzanie słownikami (autorzy, lokalizacje w sklepie). Posiada wbudowany system autoryzacji z podziałem na role.

## 🚀 Główne funkcjonalności

* **Dla Gości (Niezalogowanych):**
  * Przeglądanie aktualnego stanu magazynowego komiksów (tytuł, zeszyt, cena, dostępność).
* **Dla Pracowników i Właściciela:**
  * Pełny CRUD (Dodawanie, Edycja, Usuwanie) dla asortymentu komiksów.
  * Zarządzanie tabelami słownikowymi (dodawanie nowych Autorów oraz Lokalizacji).
* **System Uwierzytelniania:**
  * Logowanie oparte na ASP.NET Core Identity.
  * Autoryzacja dostępu do widoków i operacji na podstawie ról (`Wlasciciel`, `Pracownik`).
  * Automatyczny seeder tworzący role i domyślne konto administratora przy pierwszym uruchomieniu.

## 🛠️ Użyte Technologie

* **Framework:** .NET 10.0
* **Frontend:** Blazor Web App (Server-side rendering / Interactive Global)
* **Baza Danych:** MySQL (obsługiwana przez XAMPP)
* **ORM:** Entity Framework Core (z pakietem `Pomelo.EntityFrameworkCore.MySql`)
* **Autoryzacja:** ASP.NET Core Identity
* **Testy Automatyczne:** xUnit + EF Core InMemory Database

## ⚙️ Wymagania Wstępne

Aby uruchomić projekt na swoim komputerze, będziesz potrzebować:
* [Visual Studio 2022](https://visualstudio.microsoft.com/) (z zainstalowanym workloadem "ASP.NET and web development").
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
* [XAMPP](https://www.apachefriends.org/pl/index.html) (z uruchomionym modułem MySQL).

##Instrukcja Uruchomienia

1. **Baza Danych (XAMPP):**
   * Uruchom XAMPP Control Panel i włącz moduł **MySQL**.
   * Przejdź do phpMyAdmin (`http://localhost/phpmyadmin`) i utwórz bazę o nazwie `sklep_komiksy`.
   * Zaimportuj dostarczony plik `sklep_komiksy.sql`, aby utworzyć strukturę tabel dla produktów i słowników.

2. **Konfiguracja Aplikacji:**
   * Otwórz projekt w Visual Studio.
   * Upewnij się, że w pliku `appsettings.json` connection string wskazuje na Twoją lokalną bazę:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Port=3306;Database=sklep_komiksy;Uid=root;Pwd=;"
     }
     ```

3. **Migracje Identity (Tabele logowania):**
   * Otwórz konsolę w Visual Studio (`View` -> `Other Windows` -> `Package Manager Console`).
   * Wpisz i zatwierdź komendę:
     ```powershell
     Update-Database
     ```
   * *Uwaga: Ta komenda wygeneruje tabele dla użytkowników i ról wewnątrz Twojej bazy `sklep_komiksy`.*

4. **Uruchomienie:**
   * Wciśnij `F5` lub kliknij przycisk "Run" w Visual Studio.
   * Przy pierwszym uruchomieniu system automatycznie utworzy role i domyślne konto Właściciela.

##Domyślne Konto Logowania

Aby uzyskać dostęp do panelu zarządzania magazynem po pierwszym uruchomieniu, użyj poniższych danych:

* **Email:** `admin@sklep.pl`
* **Hasło:** `Szef123!`
* **Rola:** Właściciel

##Testy Jednostkowe

Projekt zawiera zestaw testów automatycznych weryfikujących poprawność działania logiki biznesowej (`ProduktService`). Testy są odizolowane od głównej bazy danych i wykorzystują bazę w pamięci RAM (`InMemoryDatabase`). 

Aby uruchomić testy, w Visual Studio otwórz `Test Explorer` (`Ctrl+E, T`) i kliknij **Run A
