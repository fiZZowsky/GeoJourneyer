using GeoJourneyer.Application.DTOs;

namespace GeoJourneyer.Application.Services.Interfaces;

public interface IUserService
{
    string? Register(RegisterUserDto dto);
    string? Authenticate(LoginUserDto dto);
}