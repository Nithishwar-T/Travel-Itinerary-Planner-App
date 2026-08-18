using TravelItineraryPlanner.Api.Domain.Enums;

namespace TravelItineraryPlanner.Api.Domain.Entities;

public class TripCollaborator
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public string InvitedEmail { get; set; } = string.Empty;

    public int? UserId { get; set; }

    public CollaboratorPermission PermissionLevel { get; set; }

    // Navigation properties
    public Trip Trip { get; set; } = null!;

    public User? User { get; set; }
}