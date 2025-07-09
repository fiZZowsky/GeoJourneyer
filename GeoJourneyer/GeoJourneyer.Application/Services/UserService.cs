using System.Security.Cryptography;
using System.Text;
using GeoJourneyer.Application.DTOs;
using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;
using Microsoft.AspNetCore.Http;

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

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = Hash(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Age = dto.Age,
            CountryOfOrigin = dto.CountryOfOrigin,
            Photo = dto.Photo != null ? ReadFile(dto.Photo) : null
        };

        var id = _repository.Insert(user);
        return _tokenService.CreateToken(id);
    }

    public string? Authenticate(LoginUserDto dto)
    {
        var user = _repository.GetByEmail(dto.Email);
        if (user == null) return null;
        return user.PasswordHash == Hash(dto.Password) ? _tokenService.CreateToken(user.Id) : null;
    }

    public User? GetById(int id) => _repository.GetById(id);

    private static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    private static byte[] ReadFile(IFormFile file)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        return ms.ToArray();
    }

    public IEnumerable<User> SearchByUsername(string text) => _repository.SearchByUsername(text);

    public IEnumerable<User> GetFriends(int userId) => _repository.GetFriends(userId);
}