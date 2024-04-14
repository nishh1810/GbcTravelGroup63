// BookingController.cs
using Microsoft.AspNetCore.Mvc;
using Gbc_Travel_Group63.Models;
using Gbc_Travel_Group63.Data;
using System;

public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BookingController> _logger;
    private readonly ISessionService _sessionService;

    public BookingController(ApplicationDbContext context, ILogger<BookingController> logger, ISessionService sessionService)
    {
        _context = context;
        _logger = logger;
        _sessionService = sessionService;
    }

    // Action to display booking details
    public IActionResult Book(string bookingType, int itemId)
    {
        _logger.LogInformation("Booking attempt made for bookingType: " + bookingType);
        Random rnd = new Random();

        var viewModel = new Booking
        {
            // Populate the ViewModel with details
            BookingType = bookingType,
            ItemId = itemId,
            //BookingId = rnd.Next(1, 1000000),
            IsGuest = false,
            UserId = 0
            // Add other necessary properties
        };

        return View("Book", viewModel);
    }

    // Action to confirm the booking
    [HttpPost]
    public IActionResult ConfirmBooking(Booking viewModel)
    {
        Console.WriteLine(viewModel.BookingId);
        Console.WriteLine(viewModel.IsGuest);
        Console.WriteLine(viewModel.UserId);
        Console.WriteLine(viewModel.BookingType);
        Console.WriteLine(viewModel.ItemId);
        if (ModelState.IsValid)
        { 
            var newBooking = new Booking
            {
                BookingType = viewModel.BookingType,
                ItemId = viewModel.ItemId,
                IsGuest = viewModel.IsGuest,
                BookingId = viewModel.BookingId,
                UserId = viewModel.UserId
                // Add other necessary properties
            };

            if(newBooking.IsGuest == true){
                Random rnd = new Random();
                //newBooking.UserId = rnd.Next(1, 100000000);
            }

            Console.WriteLine(newBooking);

            // Save the booking to the database
            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            // Redirect to the confirmation page with the booking ID
            return RedirectToAction("Confirmation", new { bookingId = newBooking.BookingId });
        }

        // If there are validation errors, return to the booking details page
        return View("Book", viewModel);
    }

    // Action to display the confirmation page
    public IActionResult Confirmation(int bookingId)
    {
        // Fetch the booking details based on the bookingId
        var confirmedBooking = _context.Bookings.Find(bookingId);

        // Pass the details to the confirmation view
        return View("ConfirmBooking", confirmedBooking);
    }

    // Method to fetch booking details based on bookingType and itemId
    private Booking GetBookingDetails(string bookingType, int itemId)
    {
        var details = new Booking
        {
            BookingType = bookingType,
            ItemId = itemId,
        };

        return details;
    }
    [HttpPost]
    public IActionResult BookFlight(string bookingtype,int flightid)
    {
        _logger.LogInformation("Booking attempt made for bookingType: " + bookingtype);
        Random rnd = new Random();

        var viewModel = new Booking
        {
            // Populate the ViewModel with details
            BookingType = bookingtype,
            ItemId = flightid,
            //BookingId = rnd.Next(1, 1000000),
            IsGuest = false,
            UserId = 0
            // Add other necessary properties
        };

        return PartialView("_FlightBookingConfirmation", viewModel);
    }

}
