using Entities.DTO.UserDto;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Forum.API
{
    public interface IForumUserApiRepository
    {
        Task<ForumUserDto> GetForumUser(int userId);
        Task<ForumUserDto> GetForumUserDto(int userId);
        Task<List<string>> GetUserRoles();
        Task<RegisterTableViewModel> GetUsersData();
        Task<bool> UpdateAppUser(int userId, AppUserDto appUserDto);
    }
}
