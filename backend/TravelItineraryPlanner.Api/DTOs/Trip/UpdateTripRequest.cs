using System.ComponentModel.DataAnnotations;

namespace TravelItineraryPlanner.Api.DTOs.Trip;

public class UpdateTripRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}