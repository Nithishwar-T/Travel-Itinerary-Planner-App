namespace TravelItineraryPlanner.Api.Domain.Entities;

public class Trip
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User Owner { get; set; } = null!;

    public ICollection<TripDestination> TripDestinations { get; set; } = new List<TripDestination>();

    public ICollection<ItineraryDay> ItineraryDays { get; set; } = new List<ItineraryDay>();

    public ICollection<TripCollaborator> Collaborators { get; set; } = new List<TripCollaborator>();

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}