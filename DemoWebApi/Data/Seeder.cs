using Bogus;
using DemoWebApi.Helpers;
using DemoWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.Data;

public class Seeder
{
    public static void Initialize(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        
        if (!context.Users.Any(a => a.Role == AppRole.SuperAdmin))
        {
            var admin = new User()
            {
                Id = ApiConstants.SuperAdminId,
                Email = ApiConstants.SuperAdminEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(ApiConstants.SuperAdminPassword),
                CreatedAt = DateTime.UtcNow.AddYears(-10),
                UpdatedAt = DateTime.UtcNow.AddYears(-2)
            };

            context.Users.Add(admin);
            context.SaveChanges();
        }
        
        if (!context.Users.Any(u => (u.Role == AppRole.Driver) || (u.Role == AppRole.Rider) || (u.Role == AppRole.Admin) ))
        {
            var userFaker = new Faker<User>()
                .RuleFor(x => x.Id, _ => Guid.NewGuid())
                .RuleFor(x => x.FullName, f => f.Person.FullName)
                .RuleFor(x => x.Email, (f, u) => f.Internet.Email(u.FullName.ToLower()))
                .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
                .RuleFor(x => x.PasswordHash, f => BCrypt.Net.BCrypt.HashPassword(f.Random.Word()))
                .RuleFor(x => x.Role, f => f.PickRandomWithout(AppRole.SuperAdmin))
                .RuleFor(x => x.Rating, (f, u) => u.Role is AppRole.Driver ? f.Random.Int(1,5) : 0)
                .RuleFor(x => x.CreatedAt, f => f.Date.Past(10).ToUniversalTime())
                .RuleFor(x => x.UpdatedAt, f => f.Date.Recent(2).ToUniversalTime());
            var users = userFaker.Generate(100);
            
            var driver = new User()
            {
                Id = Guid.NewGuid(),
                Email = ApiConstants.SeedDriverEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(ApiConstants.SeedDriverPassword),
                CreatedAt = DateTime.UtcNow.AddYears(-10),
                UpdatedAt = DateTime.UtcNow.AddYears(-8)
            };
            
            var client = new User()
            {
                Id = Guid.NewGuid(),
                Email = ApiConstants.SeedClientEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(ApiConstants.SeedClientPassword),
                CreatedAt = DateTime.UtcNow.AddYears(-10),
                UpdatedAt = DateTime.UtcNow.AddYears(-9)
            };
            
            users.Add(driver);
            users.Add(client);
            
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}