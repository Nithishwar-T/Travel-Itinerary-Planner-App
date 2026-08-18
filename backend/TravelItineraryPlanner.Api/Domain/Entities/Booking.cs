using TravelItineraryPlanner.Api.Domain.Enums;

namespace TravelItineraryPlanner.Api.Domain.Entities;

public class Booking
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public BookingType Type { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Cost { get; set; }

    public DateTime BookingDate { get; set; }

    // Navigation property
    public Trip Trip { get; set; } = null!;
}