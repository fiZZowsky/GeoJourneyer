namespace GeoJourneyer.Application.Services.Interfaces;

public interface IUserService
{
    int Register(string username, string password);
    int? Authenticate(string username, string password);
}