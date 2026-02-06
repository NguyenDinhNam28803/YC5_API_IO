namespace YC5_API_IO.Dto
{
    public class UpdateUserRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string NewUsername { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
