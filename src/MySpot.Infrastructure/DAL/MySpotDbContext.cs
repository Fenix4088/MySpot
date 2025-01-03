using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL;

public sealed class MySpotDbContext(DbContextOptions<MySpotDbContext> options): DbContext(options)
{
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<WeeklyParkingSpot> WeeklyParkingSpots { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}