using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Api.Data;
using TravelItineraryPlanner.Api.Domain.Entities;
using TravelItineraryPlanner.Api.Domain.Enums;
using TravelItineraryPlanner.Api.DTOs.Auth;
using TravelItineraryPlanner.Api.Helpers;
using TravelItineraryPlanner.Api.Services.Interfaces;

namespace TravelItineraryPlanner.Api.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthService(
        ApplicationDbContext context,
        JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLower();

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);

        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = UserRole.Traveler,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        var token = _jwtHelper.GenerateToken(user);

        return new AuthResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = token
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLower();

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var passwordValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = _jwtHelper.GenerateToken(user);

        return new AuthResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = token
        };
    }
}