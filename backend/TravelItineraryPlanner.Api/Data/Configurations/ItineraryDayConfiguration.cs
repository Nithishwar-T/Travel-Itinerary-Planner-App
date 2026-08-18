using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelItineraryPlanner.Api.Domain.Entities;

namespace TravelItineraryPlanner.Api.Data.Configurations;

public class ItineraryDayConfiguration
    : IEntityTypeConfiguration<ItineraryDay>
{
    public void Configure(EntityTypeBuilder<ItineraryDay> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DayNumber)
            .IsRequired();

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Trip)
            .WithMany(x => x.ItineraryDays)
            .HasForeignKey(x => x.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.TripId,
            x.DayNumber
        })
        .IsUnique();
    }
}