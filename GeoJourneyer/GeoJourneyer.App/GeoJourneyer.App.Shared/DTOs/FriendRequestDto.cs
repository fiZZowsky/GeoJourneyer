using GeoJourneyer.App.Shared.Enums;

namespace GeoJourneyer.App.Shared.DTOs;

public record FriendRequestDto(int Id, int FromUserId, int ToUserId, FriendRequestStatus Status);