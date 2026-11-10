using E5XVC8.Models;
using E5XVC8.Interfaces;

namespace E5XVC8.Services;

public class CarService : ICarService
{
    private readonly CarRentalData _data;

    public CarService(CarRentalData data) { _data = data; }

    public List<Car> GetAllCars() { return _data.Cars.ToList(); }

    public Car AddCar(string brand, string model, int year, decimal dailyPrice)
    {
        if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("A márka és a típus nem lehet üres");

        if (year < 1886 || year > DateTime.Now.Year + 1)
            throw new ArgumentException("Érvénytelen évjárat");

        // A legnagyobb meglévő azonosító alapján generálunk új ID-t.
        int nextId = _data.Cars.Count == 0 ? 1 : _data.Cars.Max(car => car.Id) + 1;

        Car car = new()
        {
            Id = nextId,
            Brand = brand,
            Model = model,
            Year = year,
            DailyPrice = dailyPrice
        };

        _data.Cars.Add(car);

        return car;
    }

    public bool RemoveCar(int carId)
    {
        Car? car = _data.Cars.FirstOrDefault(car => car.Id == carId);

        if (car is null) return false;

        // Aktív kölcsönzéssel rendelkező autót nem engedünk törölni.
        if (_data.Rentals.Any(rental => rental.CarId == carId && rental.ReturnedAt is null)) throw new InvalidOperationException("Kikölcsönzött autó nem törölhető");

        return _data.Cars.Remove(car);
    }

    public List<Car> SearchCars(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) throw new ArgumentException("A keresési kifejezés nem lehet üres");

        return _data.Cars.Where(car => car.Brand.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || car.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Car> GetCarsSortedByYear()
    {
        return _data.Cars.OrderBy(car => car.Year).ToList();
    }
}
