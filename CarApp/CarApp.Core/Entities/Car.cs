namespace CarApp.Core.Entities;

public class Car
{
    public Guid Id { get; set; }

    public string Make { get; set; } = default!;

    public string Model { get; set; } = default!;

    public int Year { get; set; }

    public string Vin { get; set; } = default!;

    public int Mileage { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }
}
