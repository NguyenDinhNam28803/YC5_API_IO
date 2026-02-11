using YC5_API_IO.Dto;

namespace YC5_API_IO.Interfaces
{
    public interface UserInterface
    {
        Task<UserInforReponse> GetUserInfor(string userId);
        Task<bool> UpdateUserInfor(string userId, string NewUsername, string Password, string NewPassword);
    }
}
