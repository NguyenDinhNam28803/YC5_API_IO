using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Dto;

namespace YC5_API_IO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly AuthInterface _authInterface;
        public AuthsController(AuthInterface authInterface) 
        {
            _authInterface = authInterface;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequestDto request)
        {
            try
            {
                var response = await _authInterface.LoginUser(request.UserName, request.Password);
                return Ok(new
                {
                    success = true,
                    message = "User logged in successfully",
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
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDto request)
        {
            try
            {
                var response = await _authInterface.RegisterUser(request.UserName, request.Password, request.Email, request.PhoneNumber);
                return Ok(new
                {
                    success = true,
                    message= "User registered successfully",
                    data= response
                });
            }
            catch (Exception ex) {
                return BadRequest(new
                {
                    success= false,
                    message= ex.Message
                });
            }
        }
    }
}
