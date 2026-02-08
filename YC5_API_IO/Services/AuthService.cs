using YC5_API_IO.Interfaces;
using YC5_API_IO.Data;
using YC5_API_IO.Dto;
using BCrypt;
using Microsoft.EntityFrameworkCore;

namespace YC5_API_IO.Services
{
    public class AuthService : AuthInterface
    {
        private readonly ApplicationDbContext _context;
            private readonly IJwtInterface _jwtService;
            public AuthService(ApplicationDbContext context, IJwtInterface jWTService)        { 
            _context = context;
            _jwtService = jWTService;
        }

        public async Task<UserInforReponse> LoginUser(string userName, string password)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
                var checkPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHasshed);

                if (user != null && checkPassword)
                {
                    var Role = _context.Roles.Where(ur => ur.RoleId == user.RoleId).Select(ur => ur.RoleId).FirstOrDefault();
                    var token = _jwtService.GenerateJwtTokens(user.UserId, Role, user.UserName);
                    var response = new UserInforReponse
                    {
                        Authorization = token,
                        UserId = user.UserId.ToString(),
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    };
                    return response;
                }
                else
                {
                    throw new Exception("Invalid username or password");
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserInforReponse> RegisterUser(string userName, string password, string email, string phoneNumber)
        {
            try
            {
                var user = _context.Users.Any(u => u.UserName == userName || u.Email == email || u.PhoneNumber == phoneNumber);
                if (user)
                {
                    throw new Exception("User with information already exists");
                }
                if (password == null || email == null || phoneNumber == null) {
                    throw new Exception("Missing required fields");
                }
                
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                var roleUser = _context.Roles.Where(r => r.RoleName == "User").Select(r => r.RoleId).FirstOrDefault();
                var newUser = new YC5_API_IO.Models.User
                {
                    UserName = userName,
                    PasswordHasshed = hashedPassword,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    RoleId = roleUser // Default role as User
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var token = _jwtService.GenerateJwtTokens(newUser.UserId, roleUser, newUser.UserName);
                return new UserInforReponse {
                    Authorization = token,
                    UserId = newUser.UserId.ToString(),
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
