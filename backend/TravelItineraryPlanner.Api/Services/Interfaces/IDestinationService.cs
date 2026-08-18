using TravelItineraryPlanner.Api.DTOs.Destination;

namespace TravelItineraryPlanner.Api.Services.Interfaces;

public interface IDestinationService
{
    Task<DestinationResponse> CreateAsync(
        int userId,
        CreateDestinationRequest request);

    Task<List<DestinationResponse>> GetAllAsync();

    Task<DestinationResponse?> GetByIdAsync(int id);

    Task<DestinationResponse?> UpdateAsync(
        int userId,
        int id,
        UpdateDestinationRequest request);

    Task<bool> DeleteAsync(
        int userId,
        int id);
}