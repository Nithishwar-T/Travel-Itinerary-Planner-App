using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelItineraryPlanner.Api.Domain.Entities;

namespace TravelItineraryPlanner.Api.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(b => b.Cost)
            .HasPrecision(18, 2);

        builder.Property(b => b.BookingDate)
            .IsRequired();

        // Trip -> Bookings
        builder.HasOne(b => b.Trip)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        // Helpful index for trip budget queries
        builder.HasIndex(b => b.TripId);
    }
}