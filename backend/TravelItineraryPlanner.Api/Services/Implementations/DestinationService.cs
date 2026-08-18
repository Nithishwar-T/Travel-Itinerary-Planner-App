using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Api.Data;
using TravelItineraryPlanner.Api.Domain.Entities;
using TravelItineraryPlanner.Api.DTOs.Destination;
using TravelItineraryPlanner.Api.Services.Interfaces;

namespace TravelItineraryPlanner.Api.Services.Implementations;

public class DestinationService : IDestinationService
{
    private readonly ApplicationDbContext _context;

    public DestinationService(ApplicationDbContext context)
    {
        _context = context;
    }

    // =====================================================
    // CREATE
    // =====================================================

    public async Task<DestinationResponse> CreateAsync(
        int userId,
        CreateDestinationRequest request)
    {
        var destination = new Destination
        {
            Name = request.Name,
            City = request.City,
            Country = request.Country,
            Description = request.Description,
            CreatedByUserId = userId
        };

        _context.Destinations.Add(destination);

        await _context.SaveChangesAsync();

        return MapToResponse(destination);
    }

    // =====================================================
    // GET ALL
    // =====================================================

    public async Task<List<DestinationResponse>> GetAllAsync()
    {
        return await _context.Destinations
            .AsNoTracking()
            .OrderBy(d => d.Country)
            .ThenBy(d => d.City)
            .ThenBy(d => d.Name)
            .Select(d => new DestinationResponse
            {
                Id = d.Id,
                Name = d.Name,
                City = d.City,
                Country = d.Country,
                Description = d.Description,
                CreatedByUserId = d.CreatedByUserId
            })
            .ToListAsync();
    }

    // =====================================================
    // GET BY ID
    // =====================================================

    public async Task<DestinationResponse?> GetByIdAsync(int id)
    {
        return await _context.Destinations
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new DestinationResponse
            {
                Id = d.Id,
                Name = d.Name,
                City = d.City,
                Country = d.Country,
                Description = d.Description,
                CreatedByUserId = d.CreatedByUserId
            })
            .FirstOrDefaultAsync();
    }

    // =====================================================
    // UPDATE
    // =====================================================

    public async Task<DestinationResponse?> UpdateAsync(
        int userId,
        int id,
        UpdateDestinationRequest request)
    {
        var destination = await _context.Destinations
            .FirstOrDefaultAsync(d =>
                d.Id == id &&
                d.CreatedByUserId == userId);

        if (destination == null)
        {
            return null;
        }

        destination.Name = request.Name;
        destination.City = request.City;
        destination.Country = request.Country;
        destination.Description = request.Description;

        await _context.SaveChangesAsync();

        return MapToResponse(destination);
    }

    // =====================================================
    // DELETE
    // =====================================================

    public async Task<bool> DeleteAsync(
        int userId,
        int id)
    {
        var destination = await _context.Destinations
            .FirstOrDefaultAsync(d =>
                d.Id == id &&
                d.CreatedByUserId == userId);

        if (destination == null)
        {
            return false;
        }

        _context.Destinations.Remove(destination);

        await _context.SaveChangesAsync();

        return true;
    }

    // =====================================================
    // MAPPING
    // =====================================================

    private static DestinationResponse MapToResponse(
        Destination destination)
    {
        return new DestinationResponse
        {
            Id = destination.Id,
            Name = destination.Name,
            City = destination.City,
            Country = destination.Country,
            Description = destination.Description,
            CreatedByUserId = destination.CreatedByUserId
        };
    }
}