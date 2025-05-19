using DemoWebApi.Data;
using DemoWebApi.DTOS;
using DemoWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.Services;

public class TripsService : ITripsService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TripsService> _logger;

    public TripsService(ApplicationDbContext context, 
        ILogger<TripsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TripResponse> CreateTripRequestAsync(TripRequest request)
    {
        var selectedDriverId = Guid.CreateVersion7();
        var userId = Guid.CreateVersion7();
        var trip = new Trip
        {
            RiderId = userId,
            DriverId = selectedDriverId,
            PickupLocation = request.PickupLocation,
            DropoffLocation = request.DropoffLocation,
            RequestTime = DateTime.UtcNow,
            Status = RideStatus.InProgress,
            Fare = 5000
        };

        _context.Trips.Add(trip);
        await _context.SaveChangesAsync();

        // Prepare response
        return new TripResponse
        {
            TripId = trip.Id,
            Driver = new DriverInfo
            {
                DriverId = selectedDriverId,
                FullName = string.Empty, 
                Rating = 5
            },
            EstimatedFare = trip.Fare,
            EstimatedTimeInMinutes = 2
        };
    }

    public async Task<Trip?> GetTripByIdAsync(Guid tripId)
    {
        var trip = await _context.Trips
            .Include(t => t.DriverId)
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip is null)
        {
            _logger.LogError("Trip with ID {TripId} not found", tripId);
        }

        return trip;
    }

    public async Task UpdateTripStatusAsync(Guid tripId, RideStatus status)
    {
        var trip = await _context.Trips.FindAsync(tripId);
        if (trip is null)
        {
            _logger.LogError("Trip with ID {TripId} not found", tripId);
        }

        trip.Status = status;

        // Update timestamps based on status
        switch (status)
        {
            case RideStatus.Requested:
            case RideStatus.Accepted:
                trip.RequestTime = DateTime.UtcNow;
                break;
            case RideStatus.Completed:
            case RideStatus.Cancelled:
                trip.EndTime = DateTime.UtcNow;
                break;
            case RideStatus.InProgress:
            default:
                break;
        }

        await _context.SaveChangesAsync();
    }
}
