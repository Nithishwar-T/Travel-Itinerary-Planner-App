using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelItineraryPlanner.Api.Domain.Entities;

namespace TravelItineraryPlanner.Api.Data.Configurations;

public class TripDestinationConfiguration
    : IEntityTypeConfiguration<TripDestination>
{
    public void Configure(EntityTypeBuilder<TripDestination> builder)
    {
        builder.HasKey(td => td.Id);

        // Trip -> TripDestination
        builder.HasOne(td => td.Trip)
            .WithMany(t => t.TripDestinations)
            .HasForeignKey(td => td.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        // Destination -> TripDestination
        builder.HasOne(td => td.Destination)
            .WithMany(d => d.TripDestinations)
            .HasForeignKey(td => td.DestinationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Prevent the same destination
        // from being added twice to the same trip
        builder.HasIndex(td => new
        {
            td.TripId,
            td.DestinationId
        })
        .IsUnique();
    }
}