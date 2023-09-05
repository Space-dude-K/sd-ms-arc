using Entities;
using Entities.Models;
using Entities.Models.Forum;
using Interfaces.User;
using Microsoft.EntityFrameworkCore;

namespace Repository.User
{
    public class ForumUserDataRepository : RepositoryBase<ForumUser, ForumContext>, IForumUserDataRepository
    {
        public ForumUserDataRepository(ForumContext forumContext) : base(forumContext)
        {
        }
        public void CreateForumUser(int appUserId)
        {
            ForumUser forumUser = new() { Id = appUserId, AppUserId = appUserId };

            Create(forumUser);
        }
        public async Task<ForumUser> GetUserAsync(int userId, bool trackChanges)
        {
            var user = await FindByCondition(u => u.Id.Equals(userId), trackChanges)
                .SingleOrDefaultAsync();

            return user;
        }
    }
}