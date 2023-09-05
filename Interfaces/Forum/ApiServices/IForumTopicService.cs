using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Forum.ApiServices
{
    public interface IForumTopicService
    {
        Task<int> CreateForumTopic(int categoryId, int forumId, ForumTopicForCreationDto topic);
        Task<bool> CreateTopicPostCounter(int topicId, ForumCounterForCreationDto forumCounterForCreationDto);
        Task<bool> DeleteForumTopic(int categoryId, int forumId, int topicId);
        Task<bool> DeleteTopicPostCounter(int topicCounterId);
        Task<List<ForumViewTopicDto>> GetForumTopics(int categoryId, int forumId);
        Task<int> GetTopicCount(int categoryId);
        Task<List<ForumViewPostDto>> GetTopicPosts(int categoryId, int forumId, int topicId, int pageNumber, int pageSize, bool getAll = false);
        Task<bool> IncreaseViewCounterForTopic(int categoryId, int forumId, int topicId);
        Task<bool> UpdateTopicCounter(int categoryId, bool incresase, int postCountToDelete = 0);
    }
}
