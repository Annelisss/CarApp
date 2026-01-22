using System;
using System.Linq;
using System.Threading.Tasks;
using CarApp.ApplicationServices.Services;
using CarApp.Core.Entities;
using CarApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarApp.CarTests
{
    public class CarServiceTests
    {
        private static AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private static Car CreateCar(string make = "Toyota", string model = "Corolla")
        {
            return new Car
            {
                Make = make,
                Model = model,
                Year = 2020,
                Vin = "TESTVIN123456789",
                Mileage = 1000,
                Price = 9999m
            };
        }

        [Fact]
        public async Task GetAllAsync_WhenDatabaseHasCars_ReturnsCars()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var service = new CarService(context);

            await service.AddAsync(CreateCar("Toyota", "Corolla"));
            await service.AddAsync(CreateCar("Seat", "Ibiza"));

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Make == "Toyota");
            Assert.Contains(result, c => c.Make == "Seat");
        }

        [Fact]
        public async Task AddAsync_WhenCarIsValid_AddsCarToDatabase()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var service = new CarService(context);

            var car = CreateCar("BMW", "X5");

            // Act
            await service.AddAsync(car);

            // Assert
            var dbCar = await context.Cars.FirstOrDefaultAsync();
            Assert.NotNull(dbCar);
            Assert.Equal("BMW", dbCar!.Make);

            Assert.NotEqual(Guid.Empty, dbCar.Id);
            Assert.NotEqual(default, dbCar.CreatedAt);
            Assert.NotEqual(default, dbCar.ModifiedAt);
        }

        [Fact]
        public async Task UpdateAsync_WhenCarExists_UpdatesFieldsAndModifiedAt()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var service = new CarService(context);

            var car = CreateCar("Audi", "A4");
            await service.AddAsync(car);

            var originalModifiedAt = car.ModifiedAt;

            // Act
            car.Model = "A6";
            await service.UpdateAsync(car);

            // Assert
            var dbCar = await context.Cars.FirstAsync(x => x.Id == car.Id);
            Assert.Equal("A6", dbCar.Model);
            Assert.True(dbCar.ModifiedAt >= originalModifiedAt);
        }

        [Fact]
        public async Task DeleteAsync_WhenCarExists_RemovesCarFromDatabase()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var service = new CarService(context);

            var car = CreateCar("Volvo", "XC60");
            await service.AddAsync(car);

            // Act
            await service.DeleteAsync(car.Id);

            // Assert
            var exists = await context.Cars.AnyAsync(x => x.Id == car.Id);
            Assert.False(exists);
        }
    }
}
