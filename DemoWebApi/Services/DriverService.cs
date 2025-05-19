using DemoWebApi.Data;
using DemoWebApi.DTOS;
using DemoWebApi.Helpers;
using DemoWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.Services;

public class DriverService : IDriverService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DriverService> _logger;

    public DriverService(ApplicationDbContext context, ILogger<DriverService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<List<DriverInfo>> GetAvailableDrivers()
    {
        return await _context.Users
            .Where(d => (d.Role == AppRole.Driver))
            .Select(d => d.MapToDriverInfo())
            .ToListAsync();
    }
}
