namespace TravelItineraryPlanner.Api.DTOs.Destination;

public class DestinationResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int CreatedByUserId { get; set; }
}   