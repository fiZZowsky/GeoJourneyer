using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    User? GetByUsername(string username);
    User? GetByEmail(string email);
    IEnumerable<User> SearchByUsername(string text);
    IEnumerable<User> GetFriends(int userId);
}