using IngetinGwAPI.CustomModels;
using IngetinGwAPI.Interfaces;
using IngetinGwAPI.Models;
using IngetinGwAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IngetinGwAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class sessionController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _config;

        public sessionController(IUserRepository repository, IRefreshTokenRepository refreshTokenRepository, IConfiguration config)
        {
            _repository = repository;
            _refreshTokenRepository = refreshTokenRepository;
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task< IActionResult> Login([FromBody] LoginRequest input, CancellationToken cancellationToken)
        {
            try
            {
                var datas = await _repository.ValidateUsers(input.Email, input.Password, cancellationToken);
                if (datas is null)
                {
                    return Unauthorized(Helpers.Fail(
                        "ERR_INVALID_CREDS",
                        "incorrect username or password"
                    ));
                }

                var accessToken = GenerateAccessToken(datas);
                //Guid.NewGuid().ToString();
                var refreshToken = Guid.NewGuid().ToString();

                await _refreshTokenRepository.CreateAsync(datas.Id, refreshToken);

                return Ok(Helpers.Success(new
                {
                    user = new { datas.Id, datas.Email, datas.Name },
                    access_token = accessToken,
                    refresh_token = refreshToken
                }));
            }
            catch (Exception ex)
            {
            }
            return BadRequest(Helpers.Fail(
                        "ERR_INVALID_CREDS",
                        "Error in generate token"
                    ));
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                // Read Authorization header
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader) ||
                    !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(Helpers.Fail(
                        "ERR_INVALID_REFRESH_TOKEN", "authorization header missing"
                    ));
                }

                // Extract refresh token
                var refreshToken = authHeader["Bearer ".Length..].Trim();

                var tokenEntity = await _refreshTokenRepository.GetValidAsync(refreshToken);

                if (tokenEntity == null)
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Ok = false,
                        Err = "ERR_INVALID_REFRESH_TOKEN",
                        Msg = "refresh token is invalid or expired"
                    });
                }

                // Optional but recommended: rotate refresh token
                await _refreshTokenRepository.RevokeAsync(tokenEntity);

                //var newAccessToken = Guid.NewGuid().ToString();
                var user = await _repository.GetUserById(tokenEntity.UsersId);
                var newAccessToken = GenerateAccessToken(user);


                return Ok(new ApiResponse<object>
                {
                    Ok = true,
                    Data = new
                    {
                        access_token = newAccessToken
                    }
                });
            }
            catch (Exception ex)
            {
            }

            return BadRequest(new ApiResponse<object>
            {
                Ok = false,
                Err = "ERR_INVALID_REFRESH_TOKEN",
                Msg = "invalid refresh token"
            });
        }

        private string GenerateAccessToken(User user)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]));

            string tempTokenLimit = jwt["AccessTokenSeconds"];

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(
                    int.Parse(jwt["AccessTokenSeconds"])), // seconds
                signingCredentials: new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /*[HttpGet("ping")]
        [AllowAnonymous]
        public async Task<IActionResult> Ping(CancellationToken cancellationToken)
        {
            //return Ok("pong");
            return Ok(Helpers.Fail(
                    "Pong",
                    "Pang"
                ));
        }*/

        /*[HttpGet("authorizetest")]
        [Authorize]
        public async Task<IActionResult> AuthorizeTest()
        {
            return Ok("This is protected data");
        }*/
    }
}
