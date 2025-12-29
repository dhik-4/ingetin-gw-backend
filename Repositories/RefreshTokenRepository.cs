using IngetinGwAPI.CustomModels;
using IngetinGwAPI.Interfaces;
using IngetinGwAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IngetinGwAPI.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public RefreshTokenRepository(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<bool> CreateRefreshAsync(int UserId, string refreshToken)
        {
            bool result = false;
            try
            {
                RefreshToken token = new RefreshToken
                {
                    UsersId = UserId,
                    Token = refreshToken,
                    IsRevoked = 0,
                    ExpiresAt = DateTime.Now.AddDays(7),
                    CreatedAt = DateTime.Now
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

        public async Task<RefreshToken> GetValidRefreshAsync(string token)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(t =>
                t.Token == token &&
                //t.IsRevoked == 0 &&
                t.ExpiresAt > DateTime.Now);
        }

        public async Task RevokeRefreshAsync(RefreshToken token)
        {
            token.IsRevoked = 1;
            await _db.SaveChangesAsync();
        }

        public async Task<bool> CreateAccessAsync(int UserId, string accessToken)
        {
            bool result = false;
            try
            {
                //var jwt = _config.GetSection("Jwt");
                //double AccessTokenSeconds = double.Parse( jwt["AccessTokenSeconds"]);
                var cVariable = _config.GetSection("CustomVariable");
                double AccessTokenSeconds = double.Parse(cVariable["AccessTokenSeconds"]);

                AccessToken token = new AccessToken
                {
                    UsersId = UserId,
                    Token = accessToken,
                    IsRevoked = 0,
                    ExpiresAt = DateTime.Now.AddSeconds(AccessTokenSeconds),
                    CreatedAt = DateTime.Now
                };
                _db.AccessTokens.Add(token);
                await _db.SaveChangesAsync();

                _db.ChangeTracker.Clear();
                result = true;
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public async Task<AccessToken> GetValidAccessAsync(string token)
        {
            return await _db.AccessTokens.FirstOrDefaultAsync(t =>
                t.Token == token &&
                //t.IsRevoked == 0 &&
                t.ExpiresAt > DateTime.Now);
        }

        public async Task RevokeAccessAsync(AccessToken token)
        {
            token.IsRevoked = 1;
            await _db.SaveChangesAsync();
        }

        public async Task<int> ValidateAccessToken(string authHeader)
        {
            int Result = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    var accessToken = authHeader["Bearer ".Length..].Trim();
                    var tokenEntity = await GetValidAccessAsync(accessToken);
                    
                    Result = tokenEntity is not null ? tokenEntity.UsersId : 0;
                }
            }
            catch { }

            return Result;
        }
    }
}
