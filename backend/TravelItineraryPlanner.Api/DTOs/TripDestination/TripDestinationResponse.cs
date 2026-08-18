namespace TravelItineraryPlanner.Api.DTOs.TripDestination;

public class TripDestinationResponse
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public int DestinationId { get; set; }

    public string DestinationName { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;
}