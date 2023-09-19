using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;

namespace Interfaces.Forum.API
{
    public interface IForumCategoryRepository
    {
        void CreateCategory(ForumCategory category);
        void DeleteCategory(ForumCategory category);
        Task<PagedList<ForumCategory>> GetAllCategoriesAsync(ForumCategoryParameters forumCategoryParameters, bool trackChanges);
        Task<IEnumerable<ForumCategory>> GetCategoriesByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        Task<ForumCategory> GetCategoryAsync(int categoryId, bool trackChanges);
    }
}