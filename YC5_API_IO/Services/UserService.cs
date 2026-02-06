using YC5_API_IO.Interfaces;
using YC5_API_IO.Dto;

namespace YC5_API_IO.Services
{
    public class UserService : UserInterface
    {
        private readonly UserInterface _userInterface;
        public UserService(UserInterface userInterface)
        {
            _userInterface = userInterface;
        }

        public async Task<UserInforReponse> GetUserInfor(string userId)
        {
            return await _userInterface.GetUserInfor(userId);
        }

        public async Task<bool> UpdateUserInfor(string userId, string NewUsername, string Password, string NewPassword)
        {
            return await _userInterface.UpdateUserInfor(userId, NewUsername, Password, NewPassword);
        }
    }
}
