using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Api.Data;
using TravelItineraryPlanner.Api.DTOs.TripDestination;
using TravelItineraryPlanner.Api.Domain.Entities;
using TravelItineraryPlanner.Api.Services.Interfaces;

namespace TravelItineraryPlanner.Api.Services.Implementations;

public class TripDestinationService : ITripDestinationService
{
    private readonly ApplicationDbContext _context;

    public TripDestinationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TripDestinationResponse> AddDestinationAsync(
        int tripId,
        AddTripDestinationRequest request)
    {
        // Check whether the trip exists
        var trip = await _context.Trips
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null)
        {
            throw new KeyNotFoundException("Trip not found.");
        }

        // Check whether the destination exists
        var destination = await _context.Destinations
            .FirstOrDefaultAsync(d => d.Id == request.DestinationId);

        if (destination == null)
        {
            throw new KeyNotFoundException("Destination not found.");
        }

        // Check whether destination is already added
        var existing = await _context.TripDestinations
            .FirstOrDefaultAsync(td =>
                td.TripId == tripId &&
                td.DestinationId == request.DestinationId);

        if (existing != null)
        {
            throw new InvalidOperationException(
                "Destination is already added to this trip.");
        }

        var tripDestination = new TripDestination
        {
            TripId = tripId,
            DestinationId = request.DestinationId
        };

        _context.TripDestinations.Add(tripDestination);

        await _context.SaveChangesAsync();

        return new TripDestinationResponse
        {
            Id = tripDestination.Id,
            TripId = tripDestination.TripId,
            DestinationId = destination.Id,
            DestinationName = destination.Name,
            City = destination.City,
            Country = destination.Country
        };
    }

    public async Task<List<TripDestinationResponse>> GetDestinationsByTripAsync(
        int tripId)
    {
        var tripExists = await _context.Trips
            .AnyAsync(t => t.Id == tripId);

        if (!tripExists)
        {
            throw new KeyNotFoundException("Trip not found.");
        }

        return await _context.TripDestinations
            .Where(td => td.TripId == tripId)
            .Include(td => td.Destination)
            .Select(td => new TripDestinationResponse
            {
                Id = td.Id,
                TripId = td.TripId,
                DestinationId = td.DestinationId,
                DestinationName = td.Destination.Name,
                City = td.Destination.City,
                Country = td.Destination.Country
            })
            .ToListAsync();
    }

    public async Task RemoveDestinationAsync(
        int tripId,
        int destinationId)
    {
        var tripDestination = await _context.TripDestinations
            .FirstOrDefaultAsync(td =>
                td.TripId == tripId &&
                td.DestinationId == destinationId);

        if (tripDestination == null)
        {
            throw new KeyNotFoundException(
                "Destination is not associated with this trip.");
        }

        _context.TripDestinations.Remove(tripDestination);

        await _context.SaveChangesAsync();
    }
}