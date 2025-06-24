using Dapper;
using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Infrastructure.Persistance;

namespace GeoJourneyer.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    protected override string TableName => "Users";

    public UserRepository(DatabaseContext context) : base(context)
    {
    }

    public User? GetByUsername(string username)
    {
        using var connection = Context.CreateConnection();
        return connection.QuerySingleOrDefault<User>($"SELECT * FROM {TableName} WHERE Username = @username", new { username });
    }
}