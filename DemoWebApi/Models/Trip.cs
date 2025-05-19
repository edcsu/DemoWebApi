using DemoWebApi.Models;

public class Trip : BaseModel
{
    public Guid RiderId { get; set; }
    public Guid DriverId { get; set; }
    public string PickupLocation { get; set; } 
    public string DropoffLocation { get; set; }
    public DateTime RequestTime { get; set; }
    public DateTime? EndTime { get; set; }
    public decimal Fare { get; set; }
    public RideStatus Status { get; set; } 
}