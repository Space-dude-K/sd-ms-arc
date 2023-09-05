using Entities.DTO.UserDto;
using Entities.ViewModels;
using Forum.ViewModels;

namespace Interfaces.Forum
{
    public interface IAuthenticationService
    {
        Task<HttpResponseMessage> Login(LoginViewModel model);
        Task<HttpResponseMessage> Register(RegisterViewModel model);
    }
}