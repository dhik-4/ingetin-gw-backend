using IngetinGwAPI.Interfaces;
using IngetinGwAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IngetinGwAPI.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _db;

        public RefreshTokenRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(int UserId, string refreshToken)
        {
            bool result = false;
            try
            {
                RefreshToken token = new RefreshToken
                {
                    UsersId = UserId,
                    Token = refreshToken,
                    IsRevoked = 0,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow
                };
                _db.RefreshTokens.Add(token);
                await _db.SaveChangesAsync();

                _db.ChangeTracker.Clear();
                result = true;
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public async Task<RefreshToken> GetValidAsync(string token)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(t =>
                t.Token == token &&
                t.IsRevoked == 0 &&
                t.ExpiresAt > DateTime.UtcNow);
        }

        public async Task RevokeAsync(RefreshToken token)
        {
            token.IsRevoked = 1;
            await _db.SaveChangesAsync();
        }
    }
}
