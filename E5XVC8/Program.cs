using E5XVC8.Interfaces;
using E5XVC8.Models;
using E5XVC8.Repositories;
using E5XVC8.Services;
using E5XVC8.UI;

namespace E5XVC8;

internal class Program
{
    private static void Main(string[] args)
    {
        string filePath = Path.Combine(
            Directory.GetCurrentDirectory(), "Data", "cars.json");

        ICarRepository repository = new JsonCarRepository(filePath);

        CarRentalData data = repository.Load();

        ICarService carService = new CarService(data);
        IRentalService rentalService = new RentalService(data);

        ConsoleMenu menu = new(carService, rentalService, repository);
        menu.Run();
    }
}