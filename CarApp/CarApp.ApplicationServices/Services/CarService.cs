using CarApp.ApplicationServices.Interfaces;
using CarApp.Core.Entities;
using CarApp.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CarApp.ApplicationServices.Services;

public class CarService : ICarService
{
    private readonly AppDbContext _context;

    public CarService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Car>> GetAllAsync()
        => await _context.Cars.AsNoTracking().ToListAsync();

    public async Task<Car?> GetByIdAsync(Guid id)
        => await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Car car)
    {
        car.Id = Guid.NewGuid();
        car.CreatedAt = DateTime.UtcNow;
        car.ModifiedAt = DateTime.UtcNow;

        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Car car)
    {
        var existing = await _context.Cars.FirstOrDefaultAsync(x => x.Id == car.Id);
        if (existing is null) return;

        existing.Make = car.Make;
        existing.Model = car.Model;
        existing.Year = car.Year;
        existing.Vin = car.Vin;
        existing.Mileage = car.Mileage;
        existing.Price = car.Price;
        existing.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
        if (existing is null) return;

        _context.Cars.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
