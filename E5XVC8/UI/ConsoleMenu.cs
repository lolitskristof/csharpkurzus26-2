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

            Console.WriteLine("A menüpont még nincs implementálva.");
        }
    }
}