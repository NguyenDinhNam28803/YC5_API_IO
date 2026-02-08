using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Dto;
using System.Security.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace YC5_API_IO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly AuthInterface _authInterface;
        private readonly IJwtInterface _jwtInterface; // Inject IJwtInterface

        public AuthsController(AuthInterface authInterface, IJwtInterface jwtInterface)
        {
            _authInterface = authInterface;
            _jwtInterface = jwtInterface; // Assign injected service
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

        [HttpPost]
        [Route("Refresh")]
        public IActionResult RefreshToken([FromBody] TokenRefreshRequestDto request)
        {
            try
            {
                var newTokens = _jwtInterface.RefreshAccessToken(request.AccessToken, request.RefreshToken);
                return Ok(new
                {
                    success = true,
                    message = "Tokens refreshed successfully",
                    data = newTokens
                });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = ex.Message
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
