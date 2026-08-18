using TravelItineraryPlanner.Api.Domain.Enums;

namespace TravelItineraryPlanner.Api.Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Trip> Trips { get; set; } = new List<Trip>();

    public ICollection<Destination> CreatedDestinations { get; set; } = new List<Destination>();

    public ICollection<TripCollaborator> Collaborations { get; set; } = new List<TripCollaborator>();

    
}