using E5XVC8.Interfaces;

namespace E5XVC8.UI;

public class ConsoleMenu
{
    private readonly ICarService _carService;
    private readonly IRentalService _rentalService;
    private readonly ICarRepository _repository;

    public ConsoleMenu(ICarService carService, IRentalService rentalService, ICarRepository repository)
    {
        _carService = carService;
        _rentalService = rentalService;
        _repository = repository;
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

                default:
                    Console.WriteLine("A menüpont még nincs implementálva.");
                    break;
            }
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
}