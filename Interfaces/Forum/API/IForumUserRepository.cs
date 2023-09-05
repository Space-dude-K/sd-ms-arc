using Entities.DTO.UserDto;
using Entities.ViewModels;

namespace Interfaces.Forum.API
{
    public interface IForumUserRepository
    {
        Task<ForumUserDto> GetForumUser(int userId);
        Task<ForumUserDto> GetForumUserDto(int userId);
        Task<List<string>> GetUserRoles();
        Task<RegisterTableViewModel> GetUsersData();
        Task<bool> UpdateAppUser(int userId, AppUserDto appUserDto);
    }
}