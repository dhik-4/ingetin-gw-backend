using IngetinGwAPI.CustomModels;
using IngetinGwAPI.Interfaces;
using IngetinGwAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace IngetinGwAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class remindersController : ControllerBase
    {
        private readonly IReminderRepository _repository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public remindersController(IReminderRepository repository, IRefreshTokenRepository refreshTokenRepository)
        {
            _repository = repository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        [HttpGet]
        [Authorize]
        //[AllowAnonymous]
        public async Task<IActionResult> ListReminders(CancellationToken cancellationToken, int limit = 10)
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


                var reminders = await _repository.ListReminders(limit, cancellationToken);
                if(reminders is null || reminders.Count <= 0)
                {
                    return BadRequest(Helpers.Fail(
                        "REMINDER_EMPTY", "There is no reminder data"
                    ));
                }

                return Ok(Helpers.Success(new {
                        reminders,
                        limit
                    }
                ));
            }
            catch (Exception ex)
            {
            }

            return BadRequest(new ApiResponse<object>
            {
                Ok = false,
                Err = "ERR_GET_REMINDER",
                Msg = "Error in get reminder"
            });

            //return Ok("This is protected data");
        }

        [HttpPost]
        //[Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> CreateReminder([FromBody] Reminder_input input)
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
