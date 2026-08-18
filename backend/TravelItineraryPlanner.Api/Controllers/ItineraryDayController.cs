using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Api.Data;
using TravelItineraryPlanner.Api.Domain.Entities;
using TravelItineraryPlanner.Api.DTOs.ItineraryDay;

namespace TravelItineraryPlanner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ItineraryDayController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ItineraryDayController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/ItineraryDay/trip/1
    [HttpGet("trip/{tripId}")]
    public async Task<ActionResult<IEnumerable<ItineraryDay>>> GetDaysByTrip(
        int tripId)
    {
        var days = await _context.ItineraryDays
            .Where(x => x.TripId == tripId)
            .OrderBy(x => x.DayNumber)
            .ToListAsync();

        return Ok(days);
    }

    // GET: api/ItineraryDay/1
    [HttpGet("{id}")]
    public async Task<ActionResult<ItineraryDay>> GetDay(int id)
    {
        var day = await _context.ItineraryDays
            .Include(x => x.Activities)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (day == null)
            return NotFound();

        return Ok(day);
    }

    // POST: api/ItineraryDay
    // POST: api/ItineraryDay
    [HttpPost]
    public async Task<ActionResult<ItineraryDay>> Create(
    CreateItineraryDayRequest request)
    {
        var trip = await _context.Trips
            .FirstOrDefaultAsync(t => t.Id == request.TripId);

        if (trip == null)
        {
            return BadRequest("Trip not found.");
        }

        var day = new ItineraryDay
        {
            TripId = request.TripId,
            DayNumber = request.DayNumber,
            Date = request.Date,
            Notes = request.Notes
        };

        _context.ItineraryDays.Add(day);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetDay),
            new { id = day.Id },
            new
            {
                day.Id,
                day.TripId,
                day.DayNumber,
                day.Date,
                day.Notes
            });
    }
    // PUT: api/ItineraryDay/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDay(
        int id,
        ItineraryDay updatedDay)
    {
        if (id != updatedDay.Id)
            return BadRequest();

        var existingDay = await _context.ItineraryDays
            .FindAsync(id);

        if (existingDay == null)
            return NotFound();

        existingDay.DayNumber = updatedDay.DayNumber;
        existingDay.Date = updatedDay.Date;
        existingDay.Notes = updatedDay.Notes;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/ItineraryDay/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDay(int id)
    {
        var day = await _context.ItineraryDays
            .FindAsync(id);

        if (day == null)
            return NotFound();

        _context.ItineraryDays.Remove(day);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}