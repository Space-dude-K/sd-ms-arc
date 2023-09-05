using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;

namespace Repository.API.Forum
{
    public interface IForumBaseRepository
    {
        void CreateForumForCategory(int categoryId, ForumBase forum);
        void DeleteForum(ForumBase forum);
        Task<PagedList<ForumBase>> GetAllForumsFromCategoryAsync(int? categoryId, ForumBaseParameters forumBaseParameters, bool trackChanges);
        Task<ForumBase> GetForumFromCategoryAsync(int categoryId, int forumId, bool trackChanges);
        Task<IEnumerable<ForumBase>> GetForumsFromCategoryByIdsAsync(int categoryId, IEnumerable<int> ids, bool trackChanges);
    }
}