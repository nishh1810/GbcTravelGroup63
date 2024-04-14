using Gbc_Travel_Group63.Data;
using Gbc_Travel_Group63.Models;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Mvc;
using Gbc_Travel_Group63.Models;
using Gbc_Travel_Group63.Data;
using Microsoft.EntityFrameworkCore;

namespace Gbc_Travel_Group63.Controllers
{
    
    public class CarController : Controller
    {
        public readonly ApplicationDbContext _db;
        private readonly ILogger<CarController> _logger;
        private readonly ISessionService _sessionService;
        public CarController(ApplicationDbContext db, ILogger<CarController> logger, ISessionService sessionService)
        {
            _db = db;
            _logger = logger;
            _sessionService = sessionService;
        }
        public IActionResult Index(string? sortDirection)
        {
            var cars = _db.Cars.AsQueryable();

            switch (sortDirection)
            {
                case "low_to_high":
                    cars = cars.OrderBy(c => (double)c.PricePerDay);
                    break;
                case "high_to_low":
                    cars = cars.OrderByDescending(c => (double)c.PricePerDay);
                    break;
                // Add more cases if needed for other sorting options

                default:
                    // Default sorting, you can change this as per your requirement
                    cars = cars.OrderBy(c => (double)c.PricePerDay);
                    break;
            }

            return View(cars.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Car project)
        {
            if (ModelState.IsValid)
            {
                _db.Cars.Add(project);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var project = _db.Cars.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,City,CarBrand,CarModel,Year,Color,PricePerDay")] Car project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(project);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        private bool ProjectExists(int id)
        {
            return _db.Cars.Any(e => e.Id == id);
        }

        [HttpGet("Car/Delete/{id:int}", Name ="CarDelete")]
        public IActionResult Delete(int id)
        {
            var project = _db.Cars.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost("Car/DeleteConfirmed/{id:int}",Name ="CarDeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var project = _db.Cars.Find(id);
            if (project != null)
            {
                _db.Cars.Remove(project);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Handle the case where the project might not be found
            return NotFound();
        }
        [HttpGet("SearchCars")]
        public async Task<IActionResult> SearchC(string location, string brand)
        {
            _logger.LogInformation("Search attempt made for flights with Pickup Location: " + location + " brand: " +
            brand );
            var carsQuery = from c in _db.Cars select c;
            bool searchPerformed = !string.IsNullOrEmpty(location) || !string.IsNullOrEmpty(brand);

            if (searchPerformed)
            {
                if (!string.IsNullOrEmpty(location))
                {
                    carsQuery = carsQuery.Where(c => c.City.Contains(location));
                }

                if (!string.IsNullOrEmpty(brand))
                {
                    carsQuery = carsQuery.Where(c => c.CarBrand.Contains(brand));
                }
            }

            var matchingCars = await carsQuery.ToListAsync();
            UserActivity ua = new UserActivity();
            ua.location = location;
            ua.brand = brand;
            _db.UserActivities.Add(ua);
            _db.SaveChanges();

            ViewData["SearchPerformed"] = searchPerformed;
            return View("SearchC", matchingCars);
        }

    }
}
