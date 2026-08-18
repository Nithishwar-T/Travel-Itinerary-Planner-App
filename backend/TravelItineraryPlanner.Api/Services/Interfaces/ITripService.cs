using TravelItineraryPlanner.Api.DTOs.Trip;

namespace TravelItineraryPlanner.Api.Services.Interfaces;

public interface ITripService
{
    Task<TripResponse> CreateTripAsync(
        int userId,
        CreateTripRequest request);

    Task<List<TripResponse>> GetMyTripsAsync(
        int userId);

    Task<TripResponse?> GetTripByIdAsync(
        int userId,
        int tripId);

    Task<TripResponse?> UpdateTripAsync(
        int userId,
        int tripId,
        UpdateTripRequest request);

    Task<bool> DeleteTripAsync(
        int userId,
        int tripId);
}