using Entities.Models.Forum;

namespace Interfaces.User
{
    public interface IForumUserDataRepository
    {
        void CreateForumUser(int appUserId);
        Task<ForumUser> GetUserAsync(int userId, bool trackChanges);
    }
}