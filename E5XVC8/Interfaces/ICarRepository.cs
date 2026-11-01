using E5XVC8.Models;

namespace E5XVC8.Interfaces;

public interface ICarRepository
{
    CarRentalData Load();
    void Save(CarRentalData data);
}
