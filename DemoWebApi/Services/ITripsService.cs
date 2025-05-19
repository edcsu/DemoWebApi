using DemoWebApi.DTOS;
using DemoWebApi.Models;

namespace DemoWebApi.Services;

public interface ITripsService
{
    Task<TripResponse> CreateTripRequestAsync(TripRequest request);
    Task<Trip?> GetTripByIdAsync(Guid tripId);
    
    Task UpdateTripStatusAsync(Guid tripId, RideStatus status);
}
