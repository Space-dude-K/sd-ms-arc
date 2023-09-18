using Api_auth.Models;

namespace Api_auth_Interfaces
{
    public interface IAuthManager
    {
        Task<string> CreateToken();
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    }
}