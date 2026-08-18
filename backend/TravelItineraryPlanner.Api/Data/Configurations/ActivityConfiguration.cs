using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelItineraryPlanner.Api.Domain.Entities;

namespace TravelItineraryPlanner.Api.Data.Configurations;

public class ActivityConfiguration
    : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Location)
            .HasMaxLength(300);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.Property(x => x.Time)
            .IsRequired(false);

        builder.Property(x => x.OrderIndex)
            .IsRequired();

        // ItineraryDay → Activities
        builder.HasOne(x => x.ItineraryDay)
            .WithMany(x => x.Activities)
            .HasForeignKey(x => x.ItineraryDayId)
            .OnDelete(DeleteBehavior.Cascade);

        // Prevent duplicate ordering within the same day
        builder.HasIndex(x => new
        {
            x.ItineraryDayId,
            x.OrderIndex
        })
        .IsUnique();
    }
}