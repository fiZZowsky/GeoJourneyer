namespace GeoJourneyer.Application.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(int userId);
}