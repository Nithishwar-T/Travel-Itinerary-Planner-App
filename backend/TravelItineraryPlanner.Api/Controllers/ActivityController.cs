using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Api.Data;
using TravelItineraryPlanner.Api.Domain.Entities;

namespace TravelItineraryPlanner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivityController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ActivityController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Activity/day/1
    [HttpGet("day/{itineraryDayId}")]
    public async Task<ActionResult<IEnumerable<Activity>>> GetActivities(
        int itineraryDayId)
    {
        var activities = await _context.Activities
            .Where(x => x.ItineraryDayId == itineraryDayId)
            .OrderBy(x => x.OrderIndex)
            .ToListAsync();

        return Ok(activities);
    }

    // GET: api/Activity/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivity(int id)
    {
        var activity = await _context.Activities
            .FirstOrDefaultAsync(x => x.Id == id);

        if (activity == null)
            return NotFound();

        return Ok(activity);
    }

    // POST: api/Activity
    [HttpPost]
    public async Task<ActionResult<Activity>> CreateActivity(
        Activity activity)
    {
        _context.Activities.Add(activity);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetActivity),
            new { id = activity.Id },
            activity);
    }

    // PUT: api/Activity/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivity(
        int id,
        Activity updatedActivity)
    {
        if (id != updatedActivity.Id)
            return BadRequest();

        var existingActivity = await _context.Activities
            .FindAsync(id);

        if (existingActivity == null)
            return NotFound();

        existingActivity.Title = updatedActivity.Title;
        existingActivity.Description = updatedActivity.Description;
        existingActivity.Location = updatedActivity.Location;
        existingActivity.Time = updatedActivity.Time;
        existingActivity.Notes = updatedActivity.Notes;
        existingActivity.OrderIndex = updatedActivity.OrderIndex;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Activity/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        var activity = await _context.Activities
            .FindAsync(id);

        if (activity == null)
            return NotFound();

        _context.Activities.Remove(activity);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}