namespace DemoWebApi.Models;

public class User : BaseModel
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // "Driver" or "Rider"
    public double? Rating { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
}