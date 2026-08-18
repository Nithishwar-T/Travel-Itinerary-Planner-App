using TravelItineraryPlanner.Api.Domain.Enums;

namespace TravelItineraryPlanner.Api.DTOs.TripCollaborator;

public class CreateTripCollaboratorRequest
{
    public string InvitedEmail { get; set; } = string.Empty;

    public CollaboratorPermission PermissionLevel { get; set; }
}