using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;

namespace YC5_API_IO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountdownsController : ControllerBase
    {
        private readonly ICountdownInterface _countdownService;

        public CountdownsController(ICountdownInterface countdownService)
        {
            _countdownService = countdownService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");
        }

        // GET: api/Countdowns
        [HttpGet]
        public async Task<IActionResult> GetCountdowns()
        {
            try
            {
                var userId = GetUserId();
                var countdowns = await _countdownService.GetCountdownsAsync(userId);
                return Ok(new
                {
                    success = true,
                    message = "Countdowns retrieved successfully",
                    data = countdowns.Select(cd => new CountdownDto
                    {
                        CountDownId = cd.CountDownId,
                        UserId = cd.UserId,
                        CountDownName = cd.CountDownName,
                        CountDownDescription = cd.CountDownDescription,
                        CountDownStatus = cd.CountDownStatus.ToString(),
                        TargetDate = cd.TargetDate
                    })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // GET: api/Countdowns/{countdownId}
        [HttpGet("{countdownId}")]
        public async Task<IActionResult> GetCountdown(string countdownId)
        {
            try
            {
                var userId = GetUserId();
                var countdown = await _countdownService.GetCountdownByIdAsync(userId, countdownId);

                if (countdown == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Countdown not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Countdown retrieved successfully",
                    data = new CountdownDto
                    {
                        CountDownId = countdown.CountDownId,
                        UserId = countdown.UserId,
                        CountDownName = countdown.CountDownName,
                        CountDownDescription = countdown.CountDownDescription,
                        CountDownStatus = countdown.CountDownStatus.ToString(),
                        TargetDate = countdown.TargetDate
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // POST: api/Countdowns
        [HttpPost]
        public async Task<IActionResult> CreateCountdown([FromBody] CreateCountdownDto createCountdownDto)
        {
            try
            {
                var userId = GetUserId();
                var newCountdown = await _countdownService.CreateCountdownAsync(userId, createCountdownDto);

                return CreatedAtAction(nameof(GetCountdown), new { countdownId = newCountdown.CountDownId }, new
                {
                    success = true,
                    message = "Countdown created successfully",
                    data = new CountdownDto
                    {
                        CountDownId = newCountdown.CountDownId,
                        UserId = newCountdown.UserId,
                        CountDownName = newCountdown.CountDownName,
                        CountDownDescription = newCountdown.CountDownDescription,
                        CountDownStatus = newCountdown.CountDownStatus.ToString(),
                        TargetDate = newCountdown.TargetDate
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // PUT: api/Countdowns/{countdownId}
        [HttpPut("{countdownId}")]
        public async Task<IActionResult> UpdateCountdown(string countdownId, [FromBody] UpdateCountdownDto updateCountdownDto)
        {
            try
            {
                var userId = GetUserId();
                var updatedCountdown = await _countdownService.UpdateCountdownAsync(userId, countdownId, updateCountdownDto);

                if (updatedCountdown == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Countdown not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Countdown updated successfully",
                    data = new CountdownDto
                    {
                        CountDownId = updatedCountdown.CountDownId,
                        UserId = updatedCountdown.UserId,
                        CountDownName = updatedCountdown.CountDownName,
                        CountDownDescription = updatedCountdown.CountDownDescription,
                        CountDownStatus = updatedCountdown.CountDownStatus.ToString(),
                        TargetDate = updatedCountdown.TargetDate
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // DELETE: api/Countdowns/{countdownId}
        [HttpDelete("{countdownId}")]
        public async Task<IActionResult> DeleteCountdown(string countdownId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _countdownService.DeleteCountdownAsync(userId, countdownId);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Countdown not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Countdown deleted successfully"
                });
            }
            catch (Exception ex)
            {
                    return BadRequest(new
                    {
                        success = false,
                        message = ex.Message
                    });
            }
        }
    }
}
