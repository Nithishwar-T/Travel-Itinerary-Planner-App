using TravelItineraryPlanner.Api.Domain.Enums;

namespace TravelItineraryPlanner.Api.DTOs.TripCollaborator;

public class UpdateTripCollaboratorRequest
{
    public CollaboratorPermission PermissionLevel { get; set; }
}