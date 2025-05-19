namespace DemoWebApi.DTOS;

public class TripResponse
{
    public int TripId { get; set; }
    public DriverInfo Driver { get; set; } = new();
    public decimal EstimatedFare { get; set; }
    public int EstimatedTimeInMinutes { get; set; }
}