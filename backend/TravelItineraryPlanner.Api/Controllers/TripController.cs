using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelItineraryPlanner.Api.DTOs.Trip;
using TravelItineraryPlanner.Api.Services.Interfaces;

namespace TravelItineraryPlanner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TripController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripController(ITripService tripService)
    {
        _tripService = tripService;
    }


    // =====================================================
    // CREATE
    // POST: api/Trip
    // =====================================================

    [HttpPost]
    public async Task<IActionResult> CreateTrip(
        [FromBody] CreateTripRequest request)
    {
        var userId = GetUserId();

        try
        {
            var result = await _tripService.CreateTripAsync(
                userId,
                request);

            return CreatedAtAction(
                nameof(GetTrip),
                new { id = result.Id },
                result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // =====================================================
    // GET ALL MY TRIPS
    // GET: api/Trip
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetMyTrips()
    {
        var userId = GetUserId();

        var trips = await _tripService.GetMyTripsAsync(userId);

        return Ok(trips);
    }


    // =====================================================
    // GET SINGLE TRIP
    // GET: api/Trip/1
    // =====================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTrip(int id)
    {
        var userId = GetUserId();

        var trip = await _tripService.GetTripByIdAsync(
            userId,
            id);

        if (trip == null)
        {
            return NotFound(new
            {
                message = "Trip not found."
            });
        }

        return Ok(trip);
    }


    // =====================================================
    // UPDATE
    // PUT: api/Trip/1
    // =====================================================

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTrip(
        int id,
        [FromBody] UpdateTripRequest request)
    {
        var userId = GetUserId();

        try
        {
            var trip = await _tripService.UpdateTripAsync(
                userId,
                id,
                request);

            if (trip == null)
            {
                return NotFound(new
                {
                    message = "Trip not found."
                });
            }

            return Ok(trip);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // =====================================================
    // DELETE
    // DELETE: api/Trip/1
    // =====================================================

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTrip(int id)
    {
        var userId = GetUserId();

        var deleted = await _tripService.DeleteTripAsync(
            userId,
            id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Trip not found."
            });
        }

        return NoContent();
    }


    // =====================================================
    // GET USER ID FROM JWT
    // =====================================================

    private int GetUserId()
    {
        var claim = User.FindFirst(
            ClaimTypes.NameIdentifier);

        if (claim == null)
        {
            throw new UnauthorizedAccessException(
                "User ID claim not found.");
        }

        return int.Parse(claim.Value);
    }
}