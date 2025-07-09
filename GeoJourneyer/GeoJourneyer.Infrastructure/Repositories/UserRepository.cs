using Dapper;
using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Infrastructure.Persistance;
using GeoJourneyer.Domain;

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

    public User? GetByEmail(string email)
    {
        using var connection = Context.CreateConnection();
        return connection.QuerySingleOrDefault<User>($"SELECT * FROM {TableName} WHERE Email = @email", new { email });
    }

    public IEnumerable<User> SearchByUsername(string text)
    {
        using var connection = Context.CreateConnection();
        return connection.Query<User>($"SELECT * FROM {TableName} WHERE Username LIKE @pattern", new { pattern = "%" + text + "%" });
    }

    public IEnumerable<User> GetFriends(int userId)
    {
        using var connection = Context.CreateConnection();
        var sql = @"SELECT u.* FROM Users u
                      JOIN FriendRequests fr ON u.Id = fr.FromUserId
                      WHERE fr.ToUserId = @userId AND fr.Status = @accepted
                      UNION
                      SELECT u.* FROM Users u
                      JOIN FriendRequests fr ON u.Id = fr.ToUserId
                      WHERE fr.FromUserId = @userId AND fr.Status = @accepted";
        return connection.Query<User>(sql, new { userId, accepted = (int)FriendRequestStatus.Accepted });
    }
}