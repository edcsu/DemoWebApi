using DemoWebApi.DTOS;
using DemoWebApi.Models;

namespace DemoWebApi.Helpers;

public static class ContractMappings
{
    public static DriverInfo MapToDriverInfo(this User user)
    {
        return new DriverInfo
        {
            FullName = user.FullName,
            DriverId = user.Id,
            Rating = user.Rating ?? 0
        };
    }
}