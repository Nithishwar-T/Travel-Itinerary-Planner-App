namespace TravelItineraryPlanner.Api.Domain.Entities;

public class ItineraryDay
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public int DayNumber { get; set; }

    public DateTime Date { get; set; }

    public string? Notes { get; set; }

    // Navigation properties
    public Trip? Trip { get; set; }

    public ICollection<Activity> Activities { get; set; }
        = new List<Activity>();
}