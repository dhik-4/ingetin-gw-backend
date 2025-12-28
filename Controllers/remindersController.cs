using IngetinGwAPI.CustomModels;
using IngetinGwAPI.Interfaces;
using IngetinGwAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IngetinGwAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class remindersController : ControllerBase
    {
        private readonly IReminderRepository _repository;

        public remindersController(IReminderRepository repository)
        {
            _repository = repository;
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
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReminder([FromBody] Reminder_input input, CancellationToken cancellationToken)
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


                var reminders = await _repository.CreateReminder(input, cancellationToken);
                if (reminders is null)
                {
                    return BadRequest(Helpers.Fail(
                        "REMINDER_FAIL", "Create reminder failed"
                    ));
                }

                return Ok(Helpers.Success(reminders));
            }
            catch (Exception ex)
            {
            }

            return BadRequest(new ApiResponse<object>
            {
                Ok = false,
                Err = "ERR_CREATE_REMINDER",
                Msg = "Error in create reminder"
            });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ViewReminders(int id, CancellationToken cancellationToken)
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


                var reminders = await _repository.ViewReminder(id, cancellationToken);
                if (reminders is null )
                {
                    return BadRequest(Helpers.Fail(
                        "REMINDER_EMPTY", "There is no reminder data"
                    ));
                }

                return Ok(Helpers.Success(reminders
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
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditReminder(int id, [FromBody] Reminder_input input, CancellationToken cancellationToken)
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


                var reminders = await _repository.EditReminder(id, input, cancellationToken);
                if (reminders is null)
                {
                    return BadRequest(Helpers.Fail(
                        "REMINDER_FAIL", "Update reminder failed"
                    ));
                }

                return Ok(Helpers.Success(reminders));
            }
            catch (Exception ex)
            {
            }

            return BadRequest(new ApiResponse<object>
            {
                Ok = false,
                Err = "ERR_EDIT_REMINDER",
                Msg = "Error in edit reminder"
            });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReminder(int id, CancellationToken cancellationToken)
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


                var reminders = await _repository.DeleteReminder(id, cancellationToken);
                if (!reminders)
                {
                    return BadRequest(Helpers.Fail(
                        "REMINDER_FAIL", "Delete reminder failed"
                    ));
                }

                return Ok(new { ok = true });
            }
            catch (Exception ex)
            {
            }

            return BadRequest(new ApiResponse<object>
            {
                Ok = false,
                Err = "ERR_DELETE_REMINDER",
                Msg = "Error in delete reminder"
            });
        }

        [HttpGet("timetounix")]
        [AllowAnonymous]
        public async Task<IActionResult> DateTimeToUnix(DateTime input1, DateTime input2)
        {
            long unix1 = new DateTimeOffset(input1).ToUnixTimeSeconds();
            long unix2 = new DateTimeOffset(input2).ToUnixTimeSeconds();

            return Ok(new
            {
                input1,
                unix1,
                input2,
                unix2
            });
        }
    }
}