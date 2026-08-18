using TravelItineraryPlanner.Api.DTOs.Auth;

namespace TravelItineraryPlanner.Api.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);
}