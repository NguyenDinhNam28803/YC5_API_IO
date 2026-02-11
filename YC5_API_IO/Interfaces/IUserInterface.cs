using YC5_API_IO.Dto;
using YC5_API_IO.Models; // Add for Models.User

namespace YC5_API_IO.Interfaces
{
    public interface IUserInterface
    {
        Task<UserInforReponse> GetUserInfor(string userId);
        Task<bool> UpdateUserInfor(string userId, string NewUsername, string Password, string NewPassword);
        Task<User?> GetUserByNameAsync(string userName); // New method
    }
}
