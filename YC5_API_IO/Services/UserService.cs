using YC5_API_IO.Interfaces;
using YC5_API_IO.Dto;
using YC5_API_IO.Data;

namespace YC5_API_IO.Services
{
    public class UserService : UserInterface
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserInforReponse> GetUserInfor(string userId)
        {
            try
            {
                var userInfor = await _context.Users.FindAsync(userId);
                if (userInfor == null)
                {
                    throw new Exception("User not found");
                }

                var response = new UserInforReponse
                {
                    UserId = userInfor.UserId,
                    UserName = userInfor.UserName,
                    Email = userInfor.Email,
                    PhoneNumber = userInfor.PhoneNumber
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateUserInfor(string userId, string NewUsername, string Password, string NewPassword)
        {
            try
            {
                var userInfor = await _context.Users.FindAsync(userId);
                if (userInfor == null)
                {
                    throw new Exception("User not found");
                }

                var checkPassword = BCrypt.Net.BCrypt.Verify(Password, userInfor.PasswordHasshed);
                if (!checkPassword)
                {
                    throw new Exception("Invalid password");
                }

                if(Password != NewPassword)
                {
                    throw new Exception("New password must be confirm");
                }

                if (!string.IsNullOrEmpty(NewPassword))
                {
                    var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(NewPassword);
                    userInfor.PasswordHasshed = hashedNewPassword;
                }

                userInfor.UserName = NewUsername;
                userInfor.Email = NewPassword;
                _context.Users.Update(userInfor);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
