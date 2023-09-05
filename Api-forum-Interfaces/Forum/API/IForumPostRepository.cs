using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;

namespace Interfaces.Forum.API
{
    public interface IForumPostRepository
    {
        void CreatePostForTopic(int forumTopicId, ForumPost post);
        void DeletePost(ForumPost post);
        Task<PagedList<ForumPost>> GetAllPostsFromCategoryAsyncForBiggerData(int? forumTopicId, ForumPostParameters forumPostParameters, bool trackChanges);
        Task<PagedList<ForumPost>> GetAllPostsFromTopicAsync(int? forumTopicId, ForumPostParameters forumPostParameters, bool getAll, bool trackChanges);
        Task<PagedList<ForumPost>> GetAllPostsFromTopicAsyncFilteredByUserId(int? forumTopicId, ForumPostParameters forumPostParameters, bool getAll, bool trackChanges);
        Task<ForumPost> GetPostAsync(int forumTopicId, int postId, bool trackChanges);
        Task<IEnumerable<ForumPost>> GetPostsFromTopicByIdsAsync(int topicId, IEnumerable<int> ids, bool trackChanges);
    }
}