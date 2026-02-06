using YC5_API_IO.Dto;

namespace YC5_API_IO.Interfaces
{
    public interface AuthInterface
    {
        Task<UserInforReponse> RegisterUser(string userName, string password, string email, string phoneNumber);
        Task<UserInforReponse> LoginUser(string userName, string password);
    }
}
