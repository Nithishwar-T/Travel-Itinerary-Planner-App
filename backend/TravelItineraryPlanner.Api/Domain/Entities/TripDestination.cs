namespace TravelItineraryPlanner.Api.Domain.Entities;

public class TripDestination
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public int DestinationId { get; set; }

    // Navigation properties
    public Trip Trip { get; set; } = null!;

    public Destination Destination { get; set; } = null!;
}