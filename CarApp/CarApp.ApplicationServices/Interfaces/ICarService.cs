using CarApp.Core.Entities;

namespace CarApp.ApplicationServices.Interfaces;

public interface ICarService
{
    Task<List<Car>> GetAllAsync();
    Task<Car?> GetByIdAsync(Guid id);
    Task AddAsync(Car car);
    Task UpdateAsync(Car car);
    Task DeleteAsync(Guid id);
}