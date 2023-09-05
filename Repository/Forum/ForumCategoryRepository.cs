using Entities;
using Interfaces.Forum;
using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore;
using Entities.RequestFeatures.Forum;
using Entities.RequestFeatures;
using System.ComponentModel.Design;
using Repository.Extensions;

namespace Repository.Forum
{
    public class ForumCategoryRepository : RepositoryBase<ForumCategory, ForumContext>, IForumCategoryRepository
    {
        public ForumCategoryRepository(ForumContext forumContext) : base(forumContext)
        {
        }

        public void CreateCategory(ForumCategory category)
        {
            Create(category);
        }
        public void DeleteCategory(ForumCategory category)
        {
            Delete(category);
        }
        public async Task<PagedList<ForumCategory>> GetAllCategoriesAsync(
            ForumCategoryParameters forumCategoryParameters, bool trackChanges)
        {
            var categories = await FindAll(trackChanges)
                .Search(forumCategoryParameters.SearchTerm)
                .Sort(forumCategoryParameters.OrderBy)
                .ToListAsync();

            return PagedList<ForumCategory>.ToPagedList(categories, forumCategoryParameters.PageNumber, forumCategoryParameters.PageSize);
        }
        public async Task<ForumCategory> GetCategoryAsync(int categoryId, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(categoryId), trackChanges)
            .SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<ForumCategory>> GetCategoriesByIdsAsync(IEnumerable<int> ids, bool trackChanges)
        {
            return await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        }
    }
}