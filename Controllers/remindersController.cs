using IngetinGwAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IngetinGwAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class remindersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _config;

        public remindersController(IUserRepository repository, IRefreshTokenRepository refreshTokenRepository, IConfiguration config)
        {
            _repository = repository;
            _refreshTokenRepository = refreshTokenRepository;
            _config = config;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListReminders(int limit = 10)
        {
            return Ok("This is protected data");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReminder()
        {
            return Ok("This is protected data");
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ViewReminders()
        {
            return Ok("This is protected data");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditReminder()
        {
            return Ok("This is protected data");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReminder()
        {
            return Ok("This is protected data");
        }
    }
}
