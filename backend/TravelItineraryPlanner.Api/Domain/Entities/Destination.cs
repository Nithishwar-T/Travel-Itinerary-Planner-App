namespace TravelItineraryPlanner.Api.Domain.Entities;

public class Destination
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int CreatedByUserId { get; set; }

    // Navigation properties
    public User CreatedByUser { get; set; } = null!;

    public ICollection<TripDestination> TripDestinations { get; set; }
        = new List<TripDestination>();

    public ICollection<Activity> Activities { get; set; }
        = new List<Activity>();
}