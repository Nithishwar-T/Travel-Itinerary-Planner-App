using System.ComponentModel.DataAnnotations;

namespace TravelItineraryPlanner.Api.DTOs.Destination;

public class CreateDestinationRequest
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }
}