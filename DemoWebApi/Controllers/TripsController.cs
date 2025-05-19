using DemoWebApi.DTOS;
using DemoWebApi.Models;
using DemoWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripsService _tripsService;

    public TripsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    [HttpPost("request")]
    public async Task<ActionResult<TripResponse>> RequestTrip([FromBody] TripRequest request)
    {
        var trip = await _tripsService.CreateTripRequestAsync(request);
        return Ok(trip);
    }

    [HttpGet("{tripId:guid}")]
    public async Task<ActionResult<Trip>> GetTrip(Guid tripId)
    {
        var trip = await _tripsService.GetTripByIdAsync(tripId);
        if (trip == null)
            return NotFound();
        return Ok(trip);
    }

    [HttpPut("{tripId:guid}/status")]
    public async Task<IActionResult> UpdateTripStatus(Guid tripId, [FromBody] RideStatus status)
    {
        await _tripsService.UpdateTripStatusAsync(tripId, status);
        return Ok();
    }
}
