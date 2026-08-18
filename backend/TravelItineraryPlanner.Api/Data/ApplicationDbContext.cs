using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Api.Domain.Entities;

namespace TravelItineraryPlanner.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<TripDestination> TripDestinations => Set<TripDestination>();
    public DbSet<ItineraryDay> ItineraryDays => Set<ItineraryDay>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<TripCollaborator> TripCollaborators => Set<TripCollaborator>();
    public DbSet<Booking> Bookings => Set<Booking>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}