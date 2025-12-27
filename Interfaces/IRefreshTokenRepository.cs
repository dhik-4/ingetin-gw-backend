using IngetinGwAPI.Models;

namespace IngetinGwAPI.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetValidAsync(string token);
        Task RevokeAsync(RefreshToken token);
        Task<bool> CreateAsync(int UserId, string refreshToken);
    }
}
