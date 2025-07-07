namespace GeoJourneyer.Application.DTOs;

public record RegisterUserDto(
    string Username,
    string Email,
    string Password,
    string FirstName = "",
    string LastName = "",
    int Age = 0,
    string CountryOfOrigin = "",
    string? PhotoUrl = null);