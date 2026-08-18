using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelItineraryPlanner.Api.Domain.Entities;

namespace TravelItineraryPlanner.Api.Data.Configurations;

public class TripCollaboratorConfiguration
    : IEntityTypeConfiguration<TripCollaborator>
{
    public void Configure(EntityTypeBuilder<TripCollaborator> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InvitedEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.PermissionLevel)
            .IsRequired();

        // Trip → Collaborators
        builder.HasOne(x => x.Trip)
            .WithMany(x => x.Collaborators)
            .HasForeignKey(x => x.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        // User → Collaborations
        builder.HasOne(x => x.User)
            .WithMany(x => x.Collaborations)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Prevent duplicate invitation for the same trip
        builder.HasIndex(x => new
        {
            x.TripId,
            x.InvitedEmail
        })
        .IsUnique();
    }
}