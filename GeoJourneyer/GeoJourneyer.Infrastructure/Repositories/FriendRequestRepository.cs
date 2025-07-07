using Dapper;
using GeoJourneyer.Application.Repositories;
using GeoJourneyer.Domain.Entities;
using GeoJourneyer.Infrastructure.Persistance;

namespace GeoJourneyer.Infrastructure.Repositories;

public class FriendRequestRepository : BaseRepository<FriendRequest>, IFriendRequestRepository
{
    protected override string TableName => "FriendRequests";

    public FriendRequestRepository(DatabaseContext context) : base(context)
    {
    }

    public FriendRequest? GetBetweenUsers(int fromUserId, int toUserId)
    {
        using var connection = Context.CreateConnection();
        return connection.QuerySingleOrDefault<FriendRequest>($"SELECT * FROM {TableName} WHERE FromUserId = @fromUserId AND ToUserId = @toUserId", new { fromUserId, toUserId });
    }
}