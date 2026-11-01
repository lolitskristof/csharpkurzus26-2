using E5XVC8.Models;

namespace E5XVC8.Services;

public class CarService
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

        if (_data.Rentals.Any(rental => rental.CarId == carId && rental.ReturnedAt is null))
            throw new InvalidOperationException("Kikölcsönzött autó nem törölhető");

        return _data.Cars.Remove(car);
    }
}
