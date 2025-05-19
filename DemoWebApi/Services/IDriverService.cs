using DemoWebApi.DTOS;

namespace DemoWebApi.Services;

public interface IDriverService
{
    Task<List<DriverInfo>> GetAvailableDrivers();
}
