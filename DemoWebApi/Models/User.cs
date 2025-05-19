namespace DemoWebApi.Models;

public class User : BaseModel
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public AppRole Role { get; set; }
    public double? Rating { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    
    public List<Trip> Trips { get; set; } = [];
}