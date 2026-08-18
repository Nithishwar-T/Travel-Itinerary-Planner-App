using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelItineraryPlanner.Api.Domain.Entities;

namespace TravelItineraryPlanner.Api.Data.Configurations;

public class TripConfiguration : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.ToTable("Trips");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.StartDate)
            .IsRequired();

        builder.Property(t => t.EndDate)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        // User 1 ─── * Trips
        builder.HasOne(t => t.Owner)
            .WithMany(u => u.Trips)
            .HasForeignKey(t => t.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Trip 1 ─── * TripDestinations
        builder.HasMany(t => t.TripDestinations)
            .WithOne(td => td.Trip)
            .HasForeignKey(td => td.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        // Trip 1 ─── * ItineraryDays
        builder.HasMany(t => t.ItineraryDays)
            .WithOne(id => id.Trip)
            .HasForeignKey(id => id.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        // Trip 1 ─── * Collaborators
        builder.HasMany(t => t.Collaborators)
            .WithOne(c => c.Trip)
            .HasForeignKey(c => c.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        // Trip 1 ─── * Bookings
        builder.HasMany(t => t.Bookings)
            .WithOne(b => b.Trip)
            .HasForeignKey(b => b.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        // Helpful index for ownership queries
        builder.HasIndex(t => t.OwnerId);
    }
}