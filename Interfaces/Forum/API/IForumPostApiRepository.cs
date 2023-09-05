using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Forum.API
{
    public interface IForumPostApiRepository
    {
        Task<int> CreateForumPost(int categoryId, int forumId, int topicId, ForumPostForCreationDto post);
        Task<bool> DeleteForumPost(int categoryId, int forumId, int topicId, int postId);
        Task<int> GetTopicPostCount(int topicId);
        Task<bool> UpdatePost(int categoryId, int forumId, int topicId, int postId, ForumPostForUpdateDto newPostDto);
        Task<bool> UpdatePostCounter(int topicId, bool incresase, int postCountToDelete = 0);
        Task<bool> UpdatePostCounterForUser(int userId, bool incresase, int postCountToDelete = 0);
        Task<bool> UpdatePostLikeCounter(int categoryId, int forumId, int topicId, int postId, bool incresase);
    }
}
