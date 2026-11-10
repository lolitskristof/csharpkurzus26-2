using E5XVC8.Models;
using E5XVC8.Interfaces;

namespace E5XVC8.Services;

public class RentalService : IRentalService
{
    private readonly CarRentalData _data;

    public RentalService(CarRentalData data)
    {
        _data = data;
    }

    public bool RentCar(int carId)
    {
        if (!_data.Cars.Any(car => car.Id == carId)) return false;

        // Ellenőrizzük, hogy az autó jelenleg ki van-e kölcsönözve.
        if (_data.Rentals.Any(rental => rental.CarId == carId && rental.ReturnedAt is null)) throw new InvalidOperationException("Az autó már ki van kölcsönözve");

        int nextId = _data.Rentals.Count == 0 ? 1 : _data.Rentals.Max(rental => rental.Id) + 1;

        Rental rental = new(nextId, carId, DateTime.Now, null);

        _data.Rentals.Add(rental);

        return true;
    }

    public bool ReturnCar(int carId)
    {
        Rental? rental = _data.Rentals.FirstOrDefault(rental => rental.CarId == carId && rental.ReturnedAt is null);

        if (rental is null) return false;

        int index = _data.Rentals.IndexOf(rental);

        // Az immutable rekord helyett új példányt hozunk létre.
        _data.Rentals[index] = rental with
        {
            ReturnedAt = DateTime.Now
        };

        return true;
    }

    public List<Car> GetAvailableCars()
    {
        // Csak az aktív kölcsönzés nélküli autókat kérjük le.
        return _data.Cars.Where(car => !_data.Rentals.Any(rental => rental.CarId == car.Id && rental.ReturnedAt is null)).ToList();
    }

    public List<Car> GetRentedCars()
    {
        return _data.Cars.Where(car => _data.Rentals.Any(rental => rental.CarId == car.Id && rental.ReturnedAt is null)).ToList();
    }
}
