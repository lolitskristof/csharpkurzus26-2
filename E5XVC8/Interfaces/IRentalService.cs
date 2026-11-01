using E5XVC8.Models;

namespace E5XVC8.Interfaces;

public interface IRentalService
{
    bool RentCar(int carId);
    bool ReturnCar(int carId);
    List<Car> GetAvailableCars();
    List<Car> GetRentedCars();
}
