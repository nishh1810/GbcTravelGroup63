using Gbc_Travel_Group63.Data;
using Gbc_Travel_Group63.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class FlightsControllercs : Controller
{
    public readonly ApplicationDbContext _db;
    private readonly ILogger<FlightsControllercs> _logger;
    private readonly ISessionService _sessionService;

    public FlightsControllercs(ApplicationDbContext db, ILogger<FlightsControllercs> logger,ISessionService sessionService)
    {
        _db = db;
        _logger = logger;
        _sessionService = sessionService;
    }
    public IActionResult Index(string? sortDirection)
    {
        var flights = _db.Flights.AsQueryable();

        switch (sortDirection)
        {
            case "low_to_high":
                flights = flights.ToList().OrderBy(f => f.Price).AsQueryable();
                break;
            case "high_to_low":
                flights = flights.ToList().OrderByDescending(f => f.Price).AsQueryable();
                break;
            // Add more cases if needed for other sorting options

            default:
                // Default sorting, you can change this as per your requirement
                flights = flights.ToList().OrderBy(f => f.Price).AsQueryable();
                break;
        }

        return View(flights.ToList());
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Flights flights)
    {
        Random rnd = new Random();
        //flights.FlightNumber = rnd.Next(1, 999999999); 
        if (ModelState.IsValid)
        {
            _db.Flights.Add(flights);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(flights);
    }
    private bool ProjectExists(int id)
    {
        return _db.Flights.Any(e => e.FlightNumber == id);
    }
    [HttpGet("FlightsControllercs/Edit/{id:int}", Name = "FlightsEdit")]
    public IActionResult Edit(int id)
    {
        var project = _db.Flights.Find(id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }
    [HttpPost("FlightsControllercs/Edit/{id:int}", Name = "FlightsEdit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("FlightNumber,DepartureCity,ArrivalCity,DepartureDate,DepartureTime,ArrivalTime,NumberOfPassengers,Price")] Flights project)
    {
        if (id != project.FlightNumber)
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
                if (!ProjectExists(project.FlightNumber))
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
        
    [HttpGet("FlightsControllercs/Delete/{id:int}",Name ="FlightsDelete")]
    public IActionResult Delete(int id)
    {
        var project = _db.Flights.FirstOrDefault(p => p.FlightNumber == id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    [HttpPost("FlightsControllercs/DeleteConfirmed/{id:int}",Name ="FlightsDeleConfirmed")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var project = _db.Flights.Find(id);
        if (project != null)
        {
            _db.Flights.Remove(project);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Handle the case where the project might not be found
        return NotFound();
    }
    [HttpGet("Search")]
    public async Task<IActionResult> SearchF(string departureCity, string arrivalCity, DateTime? departureDate)
    {

        _logger.LogInformation("Search attempt made for flights with departurecity: " + departureCity + " ArrivalCity: " +
            arrivalCity + " DepartureDate: " + departureDate);
        var flightsQuery = from f in _db.Flights select f;
        bool searchPerformed = !string.IsNullOrEmpty(departureCity) || !string.IsNullOrEmpty(arrivalCity) || departureDate.HasValue;

        if (searchPerformed)
        {
            if (!string.IsNullOrEmpty(departureCity))
            {
                flightsQuery = flightsQuery.Where(f => f.DepartureCity.Contains(departureCity));
            }

            if (!string.IsNullOrEmpty(arrivalCity))
            {
                flightsQuery = flightsQuery.Where(f => f.ArrivalCity.Contains(arrivalCity));
            }

            if (departureDate.HasValue)
            {
                flightsQuery = flightsQuery.Where(f => f.DepartureDate.Date == departureDate.Value.Date);
            }
        }

        var matchingFlights = await flightsQuery.ToListAsync();

        /// Add data to User activity
        UserActivity ua = new UserActivity();
        ua.arrivalCity = arrivalCity;
        ua.departureCity = departureCity;
        ua.DepartureDate = departureDate.Value.Date;
        _db.UserActivities.Add(ua);
        _db.SaveChanges();

        ViewData["SearchPerformed"] = searchPerformed;
        return View("SearchF", matchingFlights);

    }

    public IActionResult Book(int id)
    {
        var flight = _db.Flights.Find(id);
        
        return RedirectToAction("Book", "Booking", new { bookingType = "Flight", itemId = id });
    }

}