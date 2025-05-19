using System.Text.Json.Serialization;

namespace DemoWebApi.Models;

[JsonConverter(typeof(JsonStringEnumConverter<AppRole>))]
public enum AppRole
{
    Driver,
    Rider,
    Admin,
    SuperAdmin,
}