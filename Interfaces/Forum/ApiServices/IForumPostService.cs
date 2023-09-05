using Entities.DTO.ForumDto.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Forum.ApiServices
{
    public interface IForumPostService
    {
        Task<bool> CreateForumPost(int categoryId, int forumId, int topicId, ForumPostForCreationDto post);
        Task<bool> DeleteForumPost(int categoryId, int forumId, int topicId, int postId);
        Task<int> GetTopicPostCount(int topicId);
        Task<bool> UpdatePost(int categoryId, int forumId, int topicId, int postId, string newText);
        Task<bool> UpdatePostCounter(int topicId, bool incresase, int postCountToDelete = 0);
        Task<bool> UpdatePostCounterForUser(int userId, bool incresase, int postCountToDelete = 0);
    }
}
