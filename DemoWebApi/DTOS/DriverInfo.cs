namespace DemoWebApi.DTOS;

public class DriverInfo
{
    public Guid DriverId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public double Rating { get; set; }
}