using System.Security.Cryptography;
using System.Text;
using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository repository, ITokenService tokenService)
    {
        _repository = repository;
        _tokenService = tokenService;
    }

    public string Register(string username, string password)
    {
        var user = new User { Username = username, PasswordHash = Hash(password) };
        var id = _repository.Insert(user);
        return _tokenService.CreateToken(id);
    }

    public string? Authenticate(string username, string password)
    {
        var user = _repository.GetByUsername(username);
        if (user == null) return null;
        return user.PasswordHash == Hash(password) ? _tokenService.CreateToken(user.Id) : null;
    }

    private static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}