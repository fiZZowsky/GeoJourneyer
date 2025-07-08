namespace GeoJourneyer.App.Shared.Services;

public record FriendRequestDto(int Id, int FromUserId, int ToUserId, FriendRequestStatus Status);