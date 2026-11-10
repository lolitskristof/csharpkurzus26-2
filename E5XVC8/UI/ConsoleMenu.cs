using E5XVC8.Interfaces;
using E5XVC8.Models;
namespace E5XVC8.UI;

public class ConsoleMenu
{
    private readonly ICarService _carService;
    private readonly IRentalService _rentalService;
    private readonly ICarRepository _repository;

    private readonly CarRentalData _data;

    public ConsoleMenu(ICarService carService, IRentalService rentalService, ICarRepository repository, CarRentalData data)
    {
        _carService = carService;
        _rentalService = rentalService;
        _repository = repository;
        _data = data;
    }

    private static void ShowMenu()
    {
        Console.WriteLine("\n===== AUTÓKÖLCSÖNZŐ =====");
        Console.WriteLine("1. Autó hozzáadása");
        Console.WriteLine("2. Autó törlése");
        Console.WriteLine("3. Autók listázása");
        Console.WriteLine("4. Autók keresése");
        Console.WriteLine("5. Autók rendezése évjárat szerint");
        Console.WriteLine("6. Autó kölcsönzése");
        Console.WriteLine("7. Autó visszahozása");
        Console.WriteLine("8. Elérhető autók");
        Console.WriteLine("9. Kikölcsönzött autók");
        Console.WriteLine("0. Kilépés");
    }

    public void Run()
    {
        while (true)
        {
            ShowMenu();

            int choice = InputHelper.ReadInt("Válassz menüpontot: ");

            if (choice == 0)
            {
                Console.WriteLine("Kilépés...");
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========== EREDMÉNY ==========");
            Console.ResetColor();

            switch (choice)
            {
                case 1:
                    AddCar();
                    break;
                case 2:
                    RemoveCar();
                    break;
                case 3:
                    ListCars();
                    break;
                case 4:
                    SearchCars();
                    break;
                case 5:
                    SortCarsByYear();
                    break;
                case 6:
                    RentCar();
                    break;
                case 7:
                    ReturnCar();
                    break;
                case 8:
                    ListAvailableCars();
                    break;
                case 9:
                    ListRentedCars();
                    break;

                default:
                    Console.WriteLine("A menüpont még nincs implementálva.");
                    break;
            }

            // Az adatokat módosító műveletek után automatikusan mentünk.
            if (choice is 1 or 2 or 6 or 7)
            {
                try
                {
                    _repository.Save(_data);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Mentési hiba: {ex.Message}");
                }
            }

            Console.WriteLine("==============================");
            Console.Write("\nNyomj Entert a folytatáshoz...");
            Console.ReadLine();
            Console.Clear();
        }
    }

    private void ListCars()
    {
        var cars = _carService.GetAllCars();

        if (cars.Count == 0)
        {
            Console.WriteLine("Nincsenek autók a rendszerben.");
            return;
        }

        foreach (var car in cars)
        {
            Console.WriteLine(
                $"{car.Id} - {car.Brand} {car.Model} " +
                $"({car.Year}) - {car.DailyPrice} Ft/nap");
        }
    }

    private void AddCar()
    {
        string brand = InputHelper.ReadString("Márka: ");
        string model = InputHelper.ReadString("Típus: ");
        int year = InputHelper.ReadInt("Évjárat: ");
        decimal dailyPrice = InputHelper.ReadDecimal("Napi díj (Ft): ");

        try
        {
            var car = _carService.AddCar(brand, model, year, dailyPrice);
            Console.WriteLine($"Autó sikeresen hozzáadva! ID: {car.Id}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Hiba: {ex.Message}");
        }
    }

    private void RemoveCar()
    {
        int carId = InputHelper.ReadInt("Autó ID: ");

        try
        {
            bool removed = _carService.RemoveCar(carId);

            Console.WriteLine(removed ? "Autó sikeresen törölve." : "Nem található autó ezzel az azonosítóval.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Hiba: {ex.Message}");
        }
    }

    private void SearchCars()
    {
        string searchTerm = InputHelper.ReadString("Keresés (márka vagy típus): ");

        var cars = _carService.SearchCars(searchTerm);

        if (cars.Count == 0)
        {
            Console.WriteLine("Nincs találat.");
            return;
        }

        foreach (var car in cars)
        {
            Console.WriteLine($"{car.Id} - {car.Brand} {car.Model} " + $"({car.Year}) - {car.DailyPrice} Ft/nap");
        }
    }

    private void SortCarsByYear()
    {
        var cars = _carService.GetCarsSortedByYear();

        if (cars.Count == 0)
        {
            Console.WriteLine("Nincsenek autók a rendszerben.");
            return;
        }

        foreach (var car in cars)
        {
            Console.WriteLine($"{car.Id} - {car.Brand} {car.Model} " + $"({car.Year}) - {car.DailyPrice} Ft/nap");
        }
    }

    private void RentCar()
    {
        int carId = InputHelper.ReadInt("Autó ID: ");

        try
        {
            bool rented = _rentalService.RentCar(carId);

            Console.WriteLine(rented ? "Autó sikeresen kikölcsönözve." : "Nem található autó ezzel az azonosítóval.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Hiba: {ex.Message}");
        }
    }

    private void ReturnCar()
    {
        int carId = InputHelper.ReadInt("Autó ID: ");

        bool returned = _rentalService.ReturnCar(carId);

        Console.WriteLine(returned ? "Autó sikeresen visszahozva." : "Nem található aktív kölcsönzés ezzel az autóazonosítóval.");
    }

    private void ListAvailableCars()
    {
        var cars = _rentalService.GetAvailableCars();

        if (cars.Count == 0)
        {
            Console.WriteLine("Nincsenek elérhető autók.");
            return;
        }

        foreach (var car in cars)
        {
            Console.WriteLine($"{car.Id} - {car.Brand} {car.Model} " + $"({car.Year}) - {car.DailyPrice} Ft/nap");
        }
    }

    private void ListRentedCars()
    {
        var cars = _rentalService.GetRentedCars();

        if (cars.Count == 0)
        {
            Console.WriteLine("Nincsenek kikölcsönzött autók.");
            return;
        }

        foreach (var car in cars)
        {
            Console.WriteLine($"{car.Id} - {car.Brand} {car.Model} " + $"({car.Year}) - {car.DailyPrice} Ft/nap");
        }
    }
}