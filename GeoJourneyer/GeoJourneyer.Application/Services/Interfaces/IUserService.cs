namespace GeoJourneyer.Application.Services.Interfaces;

public interface IUserService
{
    string Register(string username, string password);
    string? Authenticate(string username, string password);
}