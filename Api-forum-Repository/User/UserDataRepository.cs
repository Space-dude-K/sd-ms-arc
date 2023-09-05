using Entities;
using Entities.Models;
using Interfaces.User;
using Microsoft.EntityFrameworkCore;
using Entities.RequestFeatures.User;
using Repository.Extensions;
using Microsoft.AspNetCore.Identity;
using Entities.Models.Forum;

namespace Repository.User
{
    public class UserDataRepository : RepositoryBase<AppUser, ForumContext>, IUserDataRepository
    {
        public UserDataRepository(ForumContext forumContext) : base(forumContext)
        {
        }
        public async Task<List<AppUser>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges)
        {
            var users = await FindAll(trackChanges)
                .Search(userParameters.SearchTerm)
                .Sort(userParameters.OrderBy)
                .ToListAsync();

            return users;
        }
        public async Task<AppUser> GetUserAsync(string userId, UserParameters userParameters, bool trackChanges)
        {
            var user = await FindByCondition(u => u.Id.Equals(Int32.Parse(userId)), trackChanges)
                .Search(userParameters.SearchTerm)
                .Sort(userParameters.OrderBy).SingleOrDefaultAsync();

            return user;
        }
        public async Task<AppUser> GetUserAsync(int userId, bool trackChanges)
        {
            var user = await FindByCondition(u => u.Id.Equals(userId), trackChanges)
                .SingleOrDefaultAsync();

            return user;
        }
    }
}