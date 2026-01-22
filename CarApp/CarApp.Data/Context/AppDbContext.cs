using CarApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarApp.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Car> Cars => Set<Car>();
}