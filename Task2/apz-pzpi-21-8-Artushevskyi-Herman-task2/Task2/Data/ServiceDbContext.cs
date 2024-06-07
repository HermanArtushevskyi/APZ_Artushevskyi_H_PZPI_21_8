using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2.Data;

public class ServiceDbContext : DbContext
{
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) { }

    public DbSet<User> UsersSet { get; set; }
    public DbSet<Token> TokensSet { get; set; }
    public DbSet<Station> StationsSet { get; set; }
    public DbSet<DroneModel> DroneModelsSet { get; set; }
    public DbSet<DroneStatus> DroneStatusesSet { get; set; }
    public DbSet<Drone> DronesSet { get; set; }
    public DbSet<Balance> BalancesSet { get; set; }
    public DbSet<DroneToStation> DronesToStationsSet { get; set; }
    public DbSet<Acceleration> AccelerationsSet { get; set; }
}