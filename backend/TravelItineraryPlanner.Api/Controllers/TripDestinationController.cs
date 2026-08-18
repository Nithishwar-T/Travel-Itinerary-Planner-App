using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelItineraryPlanner.Api.DTOs.TripDestination;
using TravelItineraryPlanner.Api.Services.Interfaces;

namespace TravelItineraryPlanner.Api.Controllers;

[ApiController]
[Route("api/Trip/{tripId}/destinations")]
[Authorize]
public class TripDestinationController : ControllerBase
{
    private readonly ITripDestinationService _service;

    public TripDestinationController(
        ITripDestinationService service)
    {
        _service = service;
    }

    // POST: api/Trip/1/destinations
    [HttpPost]
    public async Task<IActionResult> AddDestination(
        int tripId,
        [FromBody] AddTripDestinationRequest request)
    {
        try
        {
            var result = await _service.AddDestinationAsync(    
                tripId,
                request);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    // GET: api/Trip/1/destinations
    [HttpGet]
    public async Task<IActionResult> GetDestinations(
        int tripId)
    {
        try
        {
            var result = await _service
                .GetDestinationsByTripAsync(tripId);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    // DELETE: api/Trip/1/destinations/2
    [HttpDelete("{destinationId}")]
    public async Task<IActionResult> RemoveDestination(
        int tripId,
        int destinationId)
    {
        try
        {
            await _service.RemoveDestinationAsync(
                tripId,
                destinationId);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }
}