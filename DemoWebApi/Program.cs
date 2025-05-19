using System.Text;
using System.Text.Json.Serialization;
using DemoWebApi.Data;
using DemoWebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Demo API up in :{Environment}", environment);
    var builder = WebApplication.CreateBuilder(args);
    var config = builder.Configuration;
    
    // Add services to the container.
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
        options.UseNpgsql(config.GetConnectionString("DefaultConnection"), 
            opts =>
            {
                opts.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
                opts.CommandTimeout(60);
            }));
    
    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());
    
    // Configure JWT authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi(options =>
    {
    });
    builder.Services.AddScoped<ITripsService, TripsService>();

    var app = builder.Build();
    
    app.UseSerilogRequestLogging();
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            // Object initializer
            options.Title = "Demo API";
            options.ShowSidebar = true;
            options.AddPreferredSecuritySchemes("Bearer");
            options.AddHttpAuthentication("Bearer", auth =>
            {
                auth.Token = string.Empty;
            });
        });
    }

    app.UseHttpsRedirection();
    
    Seeder.Initialize(app);

    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    var type = exception.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.OrdinalIgnoreCase))
    {
        throw;
    }
    Log.Fatal(exception, "Unknown exception");
}
finally
{
    Log.Information("Demo API completely stopped");
    Log.CloseAndFlush();
}