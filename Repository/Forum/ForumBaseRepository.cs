using Interfaces.Forum;
using Entities;
using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System.ComponentModel.Design;

namespace Repository.Forum
{
    public class ForumBaseRepository : RepositoryBase<ForumBase, ForumContext>, IForumBaseRepository
    {
        public ForumBaseRepository(ForumContext forumContext) : base(forumContext)
        {
            
        }

        public void CreateForumForCategory(int categoryId, ForumBase forum)
        {
            Create(forum);
        }

        public void DeleteForum(ForumBase forum)
        {
            Delete(forum);
        }
        public async Task<PagedList<ForumBase>> GetAllForumsFromCategoryAsync(
            int? categoryId, ForumBaseParameters forumBaseParameters, bool trackChanges)
        {
            var forums = await FindByCondition(f => f.ForumCategoryId.Equals(categoryId), trackChanges)
                .Search(forumBaseParameters.SearchTerm)
                .Sort(forumBaseParameters.OrderBy)
                .ToListAsync();

            return PagedList<ForumBase>.ToPagedList(forums, forumBaseParameters.PageNumber, forumBaseParameters.PageSize);
        }
        public async Task<ForumBase> GetForumFromCategoryAsync(int categoryId, int forumId, bool trackChanges)
        {
            return await FindByCondition(c => c.ForumCategoryId.Equals(categoryId) && c.Id.Equals(forumId), trackChanges)
                .SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<ForumBase>> GetForumsFromCategoryByIdsAsync(int categoryId, IEnumerable<int> ids, bool trackChanges)
        {
            return await FindByCondition(f => f.ForumCategoryId.Equals(categoryId) && ids.Contains(f.Id), trackChanges).ToListAsync();
        }
    }
}