using TravelItineraryPlanner.Api.DTOs.TripCollaborator;

namespace TravelItineraryPlanner.Api.Services.Interfaces;

public interface ITripCollaboratorService
{
    Task<TripCollaboratorResponse> AddCollaboratorAsync(
        int tripId,
        int ownerId,
        CreateTripCollaboratorRequest request);

    Task<IEnumerable<TripCollaboratorResponse>> GetCollaboratorsAsync(
        int tripId,
        int ownerId);

    Task<TripCollaboratorResponse?> UpdateCollaboratorAsync(
        int tripId,
        int collaboratorId,
        int ownerId,
        UpdateTripCollaboratorRequest request);

    Task<bool> RemoveCollaboratorAsync(
        int tripId,
        int collaboratorId,
        int ownerId);
}