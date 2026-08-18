using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelItineraryPlanner.Api.DTOs.TripCollaborator;
using TravelItineraryPlanner.Api.Services.Interfaces;

namespace TravelItineraryPlanner.Api.Controllers;

[ApiController]
[Route("api/Trip/{tripId}/collaborators")]
[Authorize]
public class TripCollaboratorController : ControllerBase
{
    private readonly ITripCollaboratorService _service;

    public TripCollaboratorController(
        ITripCollaboratorService service)
    {
        _service = service;
    }

    // POST: api/Trip/{tripId}/collaborators
    [HttpPost]
    public async Task<ActionResult<TripCollaboratorResponse>> AddCollaborator(
        int tripId,
        CreateTripCollaboratorRequest request)
    {
        var ownerId = GetUserId();

        try
        {
            var result = await _service.AddCollaboratorAsync(
                tripId,
                ownerId,
                request);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // GET: api/Trip/{tripId}/collaborators
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TripCollaboratorResponse>>> GetCollaborators(
        int tripId)
    {
        var ownerId = GetUserId();

        try
        {
            var result = await _service.GetCollaboratorsAsync(
                tripId,
                ownerId);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // PUT: api/Trip/{tripId}/collaborators/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<TripCollaboratorResponse>> UpdateCollaborator(
        int tripId,
        int id,
        UpdateTripCollaboratorRequest request)
    {
        var ownerId = GetUserId();

        try
        {
            var result = await _service.UpdateCollaboratorAsync(
                tripId,
                id,
                ownerId,
                request);

            if (result == null)
                return NotFound(new
                {
                    message = "Collaborator not found."
                });

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // DELETE: api/Trip/{tripId}/collaborators/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveCollaborator(
        int tripId,
        int id)
    {
        var ownerId = GetUserId();

        try
        {
            var removed = await _service.RemoveCollaboratorAsync(
                tripId,
                id,
                ownerId);

            if (!removed)
                return NotFound(new
                {
                    message = "Collaborator not found."
                });

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException(
                "Invalid user identity.");

        return userId;
    }
}