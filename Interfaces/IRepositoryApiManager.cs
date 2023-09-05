using Interfaces.Forum.API;
using Repository.API.Forum;

namespace Interfaces
{
    public interface IRepositoryApiManager
    {
        IForumBaseApiRepository ForumApis { get; }
        IForumCategoryApiRepository CategoryApis { get; }
        IForumTopicApiRepository TopicApis { get; }
        IForumPostApiRepository PostApis { get; }
        IForumFileApiRepository FileApis { get; }
        IForumUserApiRepository ForumUserApis { get; }
    }
}