using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Dto;

namespace YC5_API_IO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
        public UsersController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserInfor([FromRoute] string userId)
        {
            try
            {
                var response = await _userInterface.GetUserInfor(userId);
                return Ok(new
                {
                    success = true,
                    message = "User information retrieved successfully",
                    data = response
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

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdateUserInfor([FromBody] UpdateUserRequestDto request)
        {
            try
            {
                var result = await _userInterface.UpdateUserInfor(request.UserId, request.NewUsername, request.Password, request.NewPassword);
                return Ok(new
                {
                    success = true,
                    message = "User information updated successfully",
                    data = result
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
