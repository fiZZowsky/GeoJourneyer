using GeoJourneyer.Application.DTOs;
using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Services.Interfaces;

public interface IUserService
{
    string? Register(RegisterUserDto dto);
    string? Authenticate(LoginUserDto dto);
    User? GetById(int id);
    IEnumerable<User> SearchByUsername(string text);
}