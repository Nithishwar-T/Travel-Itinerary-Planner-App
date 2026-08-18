namespace TravelItineraryPlanner.Api.DTOs.ItineraryDay;

public class CreateItineraryDayRequest
{
    public int TripId { get; set; }

    public int DayNumber { get; set; }

    public DateTime Date { get; set; }

    public string? Notes { get; set; }
}   