using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelItineraryPlanner.Api.DTOs.Destination;
using TravelItineraryPlanner.Api.Services.Interfaces;

namespace TravelItineraryPlanner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DestinationController : ControllerBase
{
    private readonly IDestinationService _destinationService;

    public DestinationController(
        IDestinationService destinationService)
    {
        _destinationService = destinationService;
    }

    // =====================================================
    // CREATE
    // POST: api/Destination
    // =====================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDestinationRequest request)
    {
        var userId = GetUserId();

        var result = await _destinationService.CreateAsync(
            userId,
            request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    // =====================================================
    // GET ALL
    // GET: api/Destination
    // =====================================================

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var destinations =
            await _destinationService.GetAllAsync();

        return Ok(destinations);
    }

    // =====================================================
    // GET BY ID
    // GET: api/Destination/1
    // =====================================================

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var destination =
            await _destinationService.GetByIdAsync(id);

        if (destination == null)
        {
            return NotFound(new
            {
                message = "Destination not found."
            });
        }

        return Ok(destination);
    }

    // =====================================================
    // UPDATE
    // PUT: api/Destination/1
    // =====================================================

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateDestinationRequest request)
    {
        var userId = GetUserId();

        var destination =
            await _destinationService.UpdateAsync(
                userId,
                id,
                request);

        if (destination == null)
        {
            return NotFound(new
            {
                message =
                    "Destination not found or you are not the creator."
            });
        }

        return Ok(destination);
    }

    // =====================================================
    // DELETE
    // DELETE: api/Destination/1
    // =====================================================

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();

        var deleted =
            await _destinationService.DeleteAsync(
                userId,
                id);

        if (!deleted)
        {
            return NotFound(new
            {
                message =
                    "Destination not found or you are not the creator."
            });
        }

        return NoContent();
    }

    // =====================================================
    // JWT USER ID
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