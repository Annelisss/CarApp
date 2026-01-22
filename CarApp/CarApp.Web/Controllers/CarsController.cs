using CarApp.ApplicationServices.Interfaces;
using CarApp.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarApp.Web.Controllers;

public class CarsController : Controller
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    public async Task<IActionResult> Index()
    {
        var cars = await _carService.GetAllAsync();
        return View(cars);
    }

    public IActionResult Create()
        => View(new Car());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Car car)
    {
        if (!ModelState.IsValid) return View(car);

        await _carService.AddAsync(car);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var car = await _carService.GetByIdAsync(id);
        if (car is null) return NotFound();

        return View(car);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var car = await _carService.GetByIdAsync(id);
        if (car is null) return NotFound();

        return View(car);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Car car)
    {
        if (!ModelState.IsValid) return View(car);

        await _carService.UpdateAsync(car);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var car = await _carService.GetByIdAsync(id);
        if (car is null) return NotFound();

        return View(car);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _carService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
