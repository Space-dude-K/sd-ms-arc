using Entities.ViewModels.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Forum
{
    public interface IForumModelService
    {
        Task<ForumHomeViewModel> GetForumCategoriesAndForumBasesForModel();
        Task<ForumBaseViewModel> GetForumTopicsForModel(int categoryId, int forumId);
        Task<ForumTopicViewModel> GetTopicPostsForModel(int categoryId, int forumId, int topicId, int pageNumber, int pageSize);
    }
}
