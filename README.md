1. Cel projektu
The Edge to zaawansowany system klasy Warehouse Management System (WMS) stworzony specjalnie dla kolekcjonerów i sprzedawców komiksów. System pozwala na precyzyjne monitorowanie stanów magazynowych, wycenę kolekcji w czasie rzeczywistym oraz śledzenie historii operacji użytkowników.

2. Stos Technologiczny (Tech Stack)
Framework: Blazor Web App (ASP.NET Core 9.0)

Język: C# 13

Baza Danych: MySQL / MariaDB

ORM: Entity Framework Core 9.0 (z użyciem sterownika Pomelo.EntityFrameworkCore.MySql v9.0.0)

Stylizacja: Tailwind CSS (Motyw: Cyberpunk Dark Mode)

Bezpieczeństwo: ASP.NET Core Identity (System ról: RBAC)

Testy: xUnit + Microsoft.EntityFrameworkCore.InMemory

3. Architektura Systemu
3.1. Model Danych (Baza Danych)
Główny model danych opiera się na dwóch kluczowych encjach:

Produkt: Zawiera szczegóły komiksu (Tytuł, Cena, Ilość, SciezkaOkladki).

HistoriaOperacji: Rejestruje każdą akcję wykonaną w systemie (Typ akcji, Użytkownik, Data, Nazwa komiksu).

3.2. Bezpieczeństwo i Role
System rozróżnia dwa poziomy uprawnień:

Administrator Systemu (Rola: Wlasciciel): Pełny dostęp do statystyk finansowych, historii operacji oraz zarządzania bazą.

Pracownik Sklepu (Rola: Pracownik): Podstawowy dostęp do listy produktów i sprzedaży, bez wglądu w logi systemowe i globalne wartości magazynu.

4. Kluczowe Funkcjonalności
4.1. Interaktywny Dashboard
KPI (Key Performance Indicators): Dynamiczne kafelki sumujące wartość magazynu, liczbę egzemplarzy i unikalnych tytułów.

System Alertów: Automatyczne wykrywanie i oznaczanie braków magazynowych (Ilość = 0).

Ostatnie Operacje: Widok historii (tylko dla Administratora) z podglądem okładek komiksów.

4.2. Zaawansowany UI/UX
Custom Login Screen: Całkowicie przeprojektowany ekran logowania z efektami "Glow" i pełną obsługą klawisza Enter.

Dynamiczne Obrazki: Integracja pola SciezkaOkladki, pozwalająca na wizualizację kolekcji.

Responsywność: Interfejs dostosowujący się do urządzeń mobilnych dzięki siatce (Grid) Tailwind.

4.3. Raportowanie PDF
Zaimplementowano moduł raportowania wykorzystujący mechanizm window.print oraz dedykowane style CSS @media print. System automatycznie:

Ukrywa elementy nawigacyjne i przyciski.

Konwertuje ciemny motyw na kontrastowy biało-czarny układ (oszczędność toneru).

Wymusza format A4 i odpowiednie marginesy dokumentu.

5. Rozwiązane Problemy Techniczne (Post-Mortem)
5.1. Konflikt Zależności (The Versioning War)
Podczas rozwoju napotkano błąd MissingMethodException. Rozwiązaniem było zsynchronizowanie wersji bibliotek Microsoftu z wersją sterownika Pomelo (wszystkie paczki EF Core zostały zablokowane na wersji 9.0.x), co zapobiegło konfliktom wewnątrz warstwy abstrakcji bazy danych.

5.2. Autoryzacja i Antiforgery
Wprowadzono dedykowane endpointy API (/api/zaloguj, /api/wyloguj) z wyłączoną walidacją Antiforgery tylko dla procesów logowania, co wyeliminowało błędy tokenów CSRF przy zachowaniu wysokiego bezpieczeństwa pozostałych części aplikacji.

6. Testowanie i Jakość Kodu
Projekt zawiera dedykowany zestaw testów jednostkowych (xUnit), które sprawdzają:

Poprawność zapisu produktów do bazy.

Automatyczne generowanie logów historii po dodaniu/usunięciu produktu.

Poprawność wyliczeń statystycznych na Dashboardzie.

Jak uruchomić projekt?
Skonfiguruj bazę MySQL w pliku appsettings.json.

Wykonaj migrację bazy: Update-Database.

Uruchom projekt: dotnet run.

System automatycznie zainicjuje role i konto administratora poprzez klasę DbSeeder.
