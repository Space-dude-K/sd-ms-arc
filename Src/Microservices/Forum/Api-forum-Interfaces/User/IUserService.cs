using Entities.DTO.UserDto;
using Entities.ViewModels;

namespace Interfaces.User
{
    public interface IUserService
    {
        Task<ForumUserDto> GetForumUser(int userId);
        Task<ForumUserDto> GetForumUserDto(int userId);
        Task<List<string>> GetUserRoles();
        Task<RegisterTableViewModel> GetUsersData();
        Task<bool> UpdateAppUser(int userId, AppUserDto appUserDto);
    }
}