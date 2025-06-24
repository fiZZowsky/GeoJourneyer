using GeoJourneyer.Domain.Entities;

namespace GeoJourneyer.Application.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    User? GetByUsername(string username);
}