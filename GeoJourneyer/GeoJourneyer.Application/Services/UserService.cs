using System.Security.Cryptography;
using System.Text;
using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public int Register(string username, string password)
    {
        var user = new User { Username = username, PasswordHash = Hash(password) };
        return _repository.Insert(user);
    }

    public int? Authenticate(string username, string password)
    {
        var user = _repository.GetByUsername(username);
        if (user == null) return null;
        return user.PasswordHash == Hash(password) ? user.Id : null;
    }

    private static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}