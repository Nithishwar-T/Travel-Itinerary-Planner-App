using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Api.Data;
using TravelItineraryPlanner.Api.Domain.Entities;
using TravelItineraryPlanner.Api.DTOs.Trip;
using TravelItineraryPlanner.Api.Services.Interfaces;

namespace TravelItineraryPlanner.Api.Services.Implementations;

public class TripService : ITripService
{
    private readonly ApplicationDbContext _context;

    public TripService(ApplicationDbContext context)
    {
        _context = context;
    }

    // =====================================================
    // CREATE TRIP
    // =====================================================

    public async Task<TripResponse> CreateTripAsync(
        int userId,
        CreateTripRequest request)
    {
        if (request.EndDate < request.StartDate)
        {
            throw new ArgumentException(
                "End date cannot be before start date.");
        }

        var trip = new Trip
        {
            OwnerId = userId,
            Title = request.Title,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedAt = DateTime.UtcNow
        };

        _context.Trips.Add(trip);

        await _context.SaveChangesAsync();

        return MapToResponse(trip);
    }


    // =====================================================
    // GET MY TRIPS
    // =====================================================

    public async Task<List<TripResponse>> GetMyTripsAsync(
        int userId)
    {
        return await _context.Trips
            .AsNoTracking()
            .Where(t => t.OwnerId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TripResponse
            {
                Id = t.Id,
                OwnerId = t.OwnerId,
                Title = t.Title,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }


    // =====================================================
    // GET SINGLE TRIP
    // =====================================================

    public async Task<TripResponse?> GetTripByIdAsync(
        int userId,
        int tripId)
    {
        return await _context.Trips
            .AsNoTracking()
            .Where(t =>
                t.Id == tripId &&
                t.OwnerId == userId)
            .Select(t => new TripResponse
            {
                Id = t.Id,
                OwnerId = t.OwnerId,
                Title = t.Title,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                CreatedAt = t.CreatedAt
            })
            .FirstOrDefaultAsync();
    }


    // =====================================================
    // UPDATE TRIP
    // =====================================================

    public async Task<TripResponse?> UpdateTripAsync(
        int userId,
        int tripId,
        UpdateTripRequest request)
    {
        if (request.EndDate < request.StartDate)
        {
            throw new ArgumentException(
                "End date cannot be before start date.");
        }

        var trip = await _context.Trips
            .FirstOrDefaultAsync(t =>
                t.Id == tripId &&
                t.OwnerId == userId);

        if (trip == null)
        {
            return null;
        }

        trip.Title = request.Title;
        trip.StartDate = request.StartDate;
        trip.EndDate = request.EndDate;

        await _context.SaveChangesAsync();

        return MapToResponse(trip);
    }


    // =====================================================
    // DELETE TRIP
    // =====================================================

    public async Task<bool> DeleteTripAsync(
        int userId,
        int tripId)
    {
        var trip = await _context.Trips
            .FirstOrDefaultAsync(t =>
                t.Id == tripId &&
                t.OwnerId == userId);

        if (trip == null)
        {
            return false;
        }

        _context.Trips.Remove(trip);

        await _context.SaveChangesAsync();

        return true;
    }


    // =====================================================
    // MAPPING
    // =====================================================

    private static TripResponse MapToResponse(Trip trip)
    {
        return new TripResponse
        {
            Id = trip.Id,
            OwnerId = trip.OwnerId,
            Title = trip.Title,
            StartDate = trip.StartDate,
            EndDate = trip.EndDate,
            CreatedAt = trip.CreatedAt
        };
    }
}