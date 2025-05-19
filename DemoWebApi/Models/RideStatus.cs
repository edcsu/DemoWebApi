using System.Text.Json.Serialization;

namespace DemoWebApi.Models;

[JsonConverter(typeof(JsonStringEnumConverter<RideStatus>))]
public enum RideStatus
{
    Requested, 
    Accepted, 
    InProgress, 
    Completed, 
    Cancelled
}