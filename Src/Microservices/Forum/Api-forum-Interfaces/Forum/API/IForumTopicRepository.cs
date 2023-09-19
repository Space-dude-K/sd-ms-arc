using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;

namespace Interfaces.Forum.API
{
    public interface IForumTopicRepository
    {
        void CreateTopicForForum(int forumBaseId, ForumTopic topic);
        void DeleteTopic(ForumTopic topic);
        Task<PagedList<ForumTopic>> GetAllTopicsFromForumAsync(int? forumBaseId, ForumTopicParameters forumTopicParameters, bool trackChanges);
        Task<ForumTopic> GetTopicAsync(int forumBaseId, int topicId, bool trackChanges);
        Task<IEnumerable<ForumTopic>> GetTopicsFromForumByIdsAsync(int forumId, IEnumerable<int> ids, bool trackChanges);
    }
}