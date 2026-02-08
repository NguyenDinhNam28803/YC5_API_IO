using System;
using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "PhoneNumber is required.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class LoginRequestDto
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }


    public class UserInforReponse 
    { 
        public JwtTokenDto? Authorization { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
