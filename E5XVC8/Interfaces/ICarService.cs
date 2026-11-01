using E5XVC8.Models;

namespace E5XVC8.Interfaces;

public interface ICarService
{
    List<Car> GetAllCars();
    Car AddCar(string brand, string model, int year, decimal dailyPrice);
    bool RemoveCar(int carId);
    List<Car> SearchCars(string searchTerm);
    List<Car> GetCarsSortedByYear();
}
