using Azure.Core;
using Hangfire;
using IngetinGwAPI.CustomModels;
using IngetinGwAPI.Interfaces;
using IngetinGwAPI.Models;
using IngetinGwAPI.Repositories;
using IngetinGwAPI.Services;
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
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public remindersController(IReminderRepository repository, IRefreshTokenRepository refreshTokenRepository,
            IBackgroundJobClient backgroundJobClient)
        {
            _repository = repository;
            _refreshTokenRepository = refreshTokenRepository;
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpGet]
        //[Authorize]
        //[AllowAnonymous]
        public async Task<IActionResult> ListReminders(CancellationToken cancellationToken, int limit = 10)
        {
            try
            {
                // Read Authorization header
                var authHeader = Request.Headers["Authorization"].ToString();

                //if (string.IsNullOrWhiteSpace(authHeader) ||
                //    !authHeader.StartsWith("Bearer "))
                //{
                //    return Unauthorized(Helpers.Fail(
                //        "ERR_INVALID_ACCESS_TOKEN", "authorization header missing"
                //    ));
                //}

                var tokenEntity = await _refreshTokenRepository.ValidateAccessToken(authHeader);
                if (tokenEntity <= 0)
                {
                    return Unauthorized(Helpers.Fail(
                        "ERR_EXPIRED_ACCESS_TOKEN", "Access token invalid or expired"
                    ));
                }

                //get list reminders
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
        //[Authorize]
        public async Task<IActionResult> CreateReminder([FromBody] Reminder_input input, CancellationToken cancellationToken)
        {
            try
            {
                // Read Authorization header
                var authHeader = Request.Headers["Authorization"].ToString();
                var tokenEntity = await _refreshTokenRepository.ValidateAccessToken(authHeader);
                if (tokenEntity <= 0)
                {
                    return Unauthorized(Helpers.Fail(
                        "ERR_ACCESS_TOKEN", "Access token invalid or expired"
                    ));
                }

                //check validity remind date and event date
                DateTime dtRemindAt = DateTimeOffset.FromUnixTimeSeconds(input.Remind_at).LocalDateTime;
                DateTime dtEventAt = DateTimeOffset.FromUnixTimeSeconds(input.Event_at).LocalDateTime;
                if (dtRemindAt <= DateTime.Now)
                {
                    return BadRequest(Helpers.Fail(
                        "REMIND_DATE_FAIL", "Remind_at cannot before date time now"
                    ));
                }
                if (dtEventAt <= DateTime.Now)
                {
                    return BadRequest(Helpers.Fail(
                        "REMIND_DATE_FAIL", "Event_at cannot before date time now"
                    ));
                }

                //create new reminder
                var reminders = await _repository.CreateReminder(input, tokenEntity, cancellationToken);
                if (reminders is null)
                {
                    return BadRequest(Helpers.Fail(
                        "REMINDER_FAIL", "Create reminder failed"
                    ));
                }

                //DateTime tempNow = DateTime.Now.AddSeconds(30);

                //start to send email
                var jobId = _backgroundJobClient.Schedule<ReminderJobService>(
                    x => x.SendReminderEmail(reminders.Id, cancellationToken),
                    dtRemindAt
                );

                //reminders.HangfireJobId = jobId;

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
        //[Authorize]
        public async Task<IActionResult> ViewReminders(int id, CancellationToken cancellationToken)
        {
            try
            {
                // Read Authorization header
                var authHeader = Request.Headers["Authorization"].ToString();
                var tokenEntity = await _refreshTokenRepository.ValidateAccessToken(authHeader);
                if (tokenEntity <= 0)
                {
                    return Unauthorized(Helpers.Fail(
                        "ERR_ACCESS_TOKEN", "Access token invalid or expired"
                    ));
                }

                //get view 1 reminder data
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
        //[Authorize]
        public async Task<IActionResult> EditReminder(int id, [FromBody] Reminder_input input, CancellationToken cancellationToken)
        {
            try
            {
                // Read Authorization header
                var authHeader = Request.Headers["Authorization"].ToString();
                var tokenEntity = await _refreshTokenRepository.ValidateAccessToken(authHeader);
                if (tokenEntity <= 0)
                {
                    return Unauthorized(Helpers.Fail(
                        "ERR_ACCESS_TOKEN", "Access token invalid or expired"
                    ));
                }

                //go update the existing reminder
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
        //[Authorize]
        public async Task<IActionResult> DeleteReminder(int id, CancellationToken cancellationToken)
        {
            try
            {
                // Read Authorization header
                var authHeader = Request.Headers["Authorization"].ToString();
                var tokenEntity = await _refreshTokenRepository.ValidateAccessToken(authHeader);
                if (tokenEntity <= 0)
                {
                    return Unauthorized(Helpers.Fail(
                        "ERR_ACCESS_TOKEN", "Access token invalid or expired"
                    ));
                }

                //go delete the reminder
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

        [HttpPost("timetounix")]
        [AllowAnonymous]
        public async Task<IActionResult> DateTimeToUnix([FromBody] DateTime[] input1)
        {
            int panjang = input1.Length;
            //long unix1 = new DateTimeOffset(input1[0]).ToUnixTimeSeconds();
            long[] unixTime = new long[panjang];
            for (int i = 0; i < panjang; i++)
            {
                unixTime[i] = new DateTimeOffset(input1[i]).ToUnixTimeSeconds();
            }
            return Ok(unixTime);
        }

        [HttpPost("unixtotime")]
        [AllowAnonymous]
        public async Task<IActionResult> UnixToDateTime([FromBody] long[] input)
        {
            DateTime[] dateTimesUTC = new DateTime[input.Length];
            DateTime[] dateTimesLocal = new DateTime[input.Length];
            
            for (int i = 0; i < input.Length; i++)
            {
                dateTimesUTC[i] = DateTimeOffset.FromUnixTimeSeconds(input[i]).UtcDateTime;
                dateTimesLocal[i] = DateTimeOffset.FromUnixTimeSeconds(input[i]).LocalDateTime;
            }

            return Ok(new { dateTimesLocal, dateTimesUTC });
        }
    }
}