using IngetinGwAPI.Models;

namespace IngetinGwAPI.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetValidRefreshAsync(string token);
        Task RevokeRefreshAsync(RefreshToken token);
        Task<bool> CreateRefreshAsync(int UserId, string refreshToken);

        Task<bool> CreateAccessAsync(int UserId, string accessToken);
        Task<AccessToken> GetValidAccessAsync(string token);
        Task RevokeAccessAsync(AccessToken token);
        Task<bool> ValidateAccessToken(string authHeader);
    }
}
