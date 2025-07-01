using System.Security.Cryptography;
using System.Text;
using GeoJourneyer.Application.DTOs;
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

    public string? Register(RegisterUserDto dto)
    {
        if (_repository.GetByUsername(dto.Username) != null)
        {
            return null;
        }

        var user = new User { Username = dto.Username, PasswordHash = Hash(dto.Password) };
        var id = _repository.Insert(user);
        return _tokenService.CreateToken(id);
    }

    public string? Authenticate(LoginUserDto dto)
    {
        var user = _repository.GetByEmail(dto.Email);
        if (user == null) return null;
        return user.PasswordHash == Hash(dto.Password) ? _tokenService.CreateToken(user.Id) : null;
    }

    private static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}