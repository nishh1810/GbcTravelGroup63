using Gbc_Travel_Group63.Data;
using Gbc_Travel_Group63.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gbc_Travel_Group63.Controllers
{
    public class HotelsController : Controller
    {
        public readonly ApplicationDbContext _db;
        private readonly ILogger<HotelsController> _logger;
        private readonly ISessionService _sessionService;
        public HotelsController(ApplicationDbContext db, ILogger<HotelsController> logger, ISessionService sessionService)
        {
            _db = db;
            _logger = logger;
            _sessionService = sessionService;
        }
        public IActionResult Index(string? sortDirection)
{
    var hotels = _db.Hotels.AsQueryable();

    switch (sortDirection)
    {
        case "low_to_high":
            hotels = hotels.OrderBy(h => (double)h.PricePerNight);
            break;
        case "high_to_low":
            hotels = hotels.OrderByDescending(h => (double)h.PricePerNight);
            break;

        default:
            hotels = hotels.OrderBy(h => (double)h.PricePerNight);
            break;
    }

    return View(hotels.ToList());
}

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hotel project)
        {
            if (ModelState.IsValid)
            {
                _db.Hotels.Add(project);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }
        [HttpGet("Hotels/Edit/{id:int}", Name = "HotelsEdit")]
        public IActionResult Edit(int id)
        {
            var project = _db.Hotels.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
        [HttpPost("Hotels/Edit/{id:int}", Name = "HotelsEdit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("HotelId,HotelName,Location,StarRating,PricePerNight,IsPetFriendly,RoomType")] Hotel project)
        {
            if (id != project.HotelId)
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
                    if (!ProjectExists(project.HotelId))
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
            return _db.Hotels.Any(e => e.HotelId == id);
        }

        [HttpGet("Hotels/Delete/{id:int}",Name ="HotelsDelete")]
        public IActionResult Delete(int id)
        {
            var project = _db.Hotels.FirstOrDefault(p => p.HotelId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost("Hotels/DeleteConfirmed/{id:int}", Name ="DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var project = _db.Hotels.Find(id);
            if (project != null)
            {
                _db.Hotels.Remove(project);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Handle the case where the project might not be found
            return NotFound();
        }
        [HttpGet("SearchHotels")]
        public async Task<IActionResult> SearchH(string location, string roomType)
        {
            _logger.LogInformation("Search attempt made for flights with location: " + location + " RoomType: " +
            roomType);
            var hotelsQuery = from h in _db.Hotels select h;
            bool searchPerformed = !string.IsNullOrEmpty(location) || !string.IsNullOrEmpty(roomType);

            if (searchPerformed)
            {
                if (!string.IsNullOrEmpty(location))
                {
                    hotelsQuery = hotelsQuery.Where(h => h.Location.Contains(location));
                }

                if (!string.IsNullOrEmpty(roomType))
                {
                    hotelsQuery = hotelsQuery.Where(h => h.RoomType.Contains(roomType));
                }
            }

            var matchingHotels = await hotelsQuery.ToListAsync();

            /// Add data to User activity
            UserActivity ua = new UserActivity();
            ua.location = location;
            ua.roomType = roomType;
            _db.UserActivities.Add(ua);
            _db.SaveChanges();

            ViewData["SearchPerformed"] = searchPerformed;
            return View("SearchH", matchingHotels);
        }

    }
}
