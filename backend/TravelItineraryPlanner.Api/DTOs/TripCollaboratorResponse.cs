using TravelItineraryPlanner.Api.Domain.Enums;

namespace TravelItineraryPlanner.Api.DTOs.TripCollaborator;

public class TripCollaboratorResponse
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public string InvitedEmail { get; set; } = string.Empty;

    public int? UserId { get; set; }

    public CollaboratorPermission PermissionLevel { get; set; }
}