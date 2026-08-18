using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelItineraryPlanner.Api.Domain.Entities;

public class Activity
{
    public int Id { get; set; }

    public int ItineraryDayId { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public TimeSpan? Time { get; set; }

    public string? Notes { get; set; }

    public int OrderIndex { get; set; }

    [ValidateNever]
    public ItineraryDay? ItineraryDay { get; set; }
}