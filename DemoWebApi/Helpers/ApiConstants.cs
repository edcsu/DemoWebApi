namespace DemoWebApi.Helpers;

public static class ApiConstants
{
    public static readonly List<decimal> Fares = [2000, 3000, 4000, 5000, 6000, 9000, 10000, 20000, 30000, 40000, 50000, 60000, 70000, 80000, 90000, 100000];
    public static readonly List<int> TimeInMinutes = [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
    public static readonly Guid SuperAdminId = Guid.Parse("669df1f7-85b6-4499-a0fe-1b176c2a0f4c");
    public const string SuperAdminEmail = "superadmin@kampaladevops.com";
    public const string SuperAdminPassword = "Admin@2025";
    public const string SeedDriverEmail = "driver@kampaladevops.com";
    public const string SeedDriverPassword = "driver@2025";
    public const string SeedClientEmail = "client@kampaladevops.com";
    public const string SeedClientPassword = "client@2025";
}