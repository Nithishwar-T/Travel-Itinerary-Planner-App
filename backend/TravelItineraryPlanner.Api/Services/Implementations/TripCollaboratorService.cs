using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Api.Data;
using TravelItineraryPlanner.Api.Domain.Entities;
using TravelItineraryPlanner.Api.DTOs.TripCollaborator;
using TravelItineraryPlanner.Api.Services.Interfaces;

namespace TravelItineraryPlanner.Api.Services.Implementations;

public class TripCollaboratorService : ITripCollaboratorService
{
    private readonly ApplicationDbContext _context;

    public TripCollaboratorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TripCollaboratorResponse> AddCollaboratorAsync(
        int tripId,
        int ownerId,
        CreateTripCollaboratorRequest request)
    {
        // Check whether the trip belongs to the logged-in user
        var trip = await _context.Trips
            .FirstOrDefaultAsync(x =>
                x.Id == tripId &&
                x.OwnerId == ownerId);

        if (trip == null)
            throw new KeyNotFoundException("Trip not found.");

        // Check duplicate invitation
        var alreadyExists = await _context.TripCollaborators
            .AnyAsync(x =>
                x.TripId == tripId &&
                x.InvitedEmail == request.InvitedEmail);

        if (alreadyExists)
            throw new InvalidOperationException(
                "This email is already a collaborator for this trip.");

        // If the invited person already has an account,
        // associate their UserId automatically.
        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.Email == request.InvitedEmail);

        var collaborator = new TripCollaborator
        {
            TripId = tripId,
            InvitedEmail = request.InvitedEmail,
            UserId = user?.Id,
            PermissionLevel = request.PermissionLevel
        };

        _context.TripCollaborators.Add(collaborator);

        await _context.SaveChangesAsync();

        return MapToResponse(collaborator);
    }

    public async Task<IEnumerable<TripCollaboratorResponse>> GetCollaboratorsAsync(
        int tripId,
        int ownerId)
    {
        // Only the trip owner can view collaborators
        var tripExists = await _context.Trips
            .AnyAsync(x =>
                x.Id == tripId &&
                x.OwnerId == ownerId);

        if (!tripExists)
            throw new KeyNotFoundException("Trip not found.");

        var collaborators = await _context.TripCollaborators
            .Where(x => x.TripId == tripId)
            .ToListAsync();

        return collaborators.Select(MapToResponse);
    }

    public async Task<TripCollaboratorResponse?> UpdateCollaboratorAsync(
        int tripId,
        int collaboratorId,
        int ownerId,
        UpdateTripCollaboratorRequest request)
    {
        var tripExists = await _context.Trips
            .AnyAsync(x =>
                x.Id == tripId &&
                x.OwnerId == ownerId);

        if (!tripExists)
            throw new KeyNotFoundException("Trip not found.");

        var collaborator = await _context.TripCollaborators
            .FirstOrDefaultAsync(x =>
                x.Id == collaboratorId &&
                x.TripId == tripId);

        if (collaborator == null)
            return null;

        collaborator.PermissionLevel = request.PermissionLevel;

        await _context.SaveChangesAsync();

        return MapToResponse(collaborator);
    }

    public async Task<bool> RemoveCollaboratorAsync(
        int tripId,
        int collaboratorId,
        int ownerId)
    {
        var tripExists = await _context.Trips
            .AnyAsync(x =>
                x.Id == tripId &&
                x.OwnerId == ownerId);

        if (!tripExists)
            throw new KeyNotFoundException("Trip not found.");

        var collaborator = await _context.TripCollaborators
            .FirstOrDefaultAsync(x =>
                x.Id == collaboratorId &&
                x.TripId == tripId);

        if (collaborator == null)
            return false;

        _context.TripCollaborators.Remove(collaborator);

        await _context.SaveChangesAsync();

        return true;
    }

    private static TripCollaboratorResponse MapToResponse(
        TripCollaborator collaborator)
    {
        return new TripCollaboratorResponse
        {
            Id = collaborator.Id,
            TripId = collaborator.TripId,
            InvitedEmail = collaborator.InvitedEmail,
            UserId = collaborator.UserId,
            PermissionLevel = collaborator.PermissionLevel
        };
    }
}