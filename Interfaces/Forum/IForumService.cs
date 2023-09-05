using Entities.DTO.FileDto;
using Entities.DTO.UserDto;

namespace Interfaces.Forum
{
    public interface IForumService
    {
        Task<bool> CreateForumFile(ForumFileDto file);
        Task<ForumFileDto> GetForumFileByUserId(int forumUserId);
        Task<ForumUserDto> GetForumUser(int userId);
        Task<bool> UpdateForumFile(int forumUserId, ForumFileDto forumFileDto);
    }
}