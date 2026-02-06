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
        private readonly UserInterface _userInterface;
        public UsersController(UserInterface userInterface)
        {
            _userInterface = userInterface;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserInfor([FromRoute] string userId)
        {
            var response = await _userInterface.GetUserInfor(userId);
            return Ok(response);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdateUserInfor([FromBody] UpdateUserRequestDto request)
        {
            var result = await _userInterface.UpdateUserInfor(request.UserId, request.NewUsername, request.Password, request.NewPassword);
            return Ok(result);
        }
    }
}
