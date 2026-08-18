using TravelItineraryPlanner.Api.DTOs.TripDestination;

namespace TravelItineraryPlanner.Api.Services.Interfaces;

public interface ITripDestinationService
{
    Task<TripDestinationResponse> AddDestinationAsync(
        int tripId,
        AddTripDestinationRequest request);

    Task<List<TripDestinationResponse>> GetDestinationsByTripAsync(
        int tripId);

    Task RemoveDestinationAsync(
        int tripId,
        int destinationId);
}