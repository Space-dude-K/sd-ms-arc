using Interfaces.File;
using Interfaces.Forum;
using Interfaces.Forum.API;
using Interfaces.User;
using Repository.API.Forum;

namespace Interfaces
{
    public interface IRepositoryManager
    {
        IRoleRepository Roles { get; }
        IForumUserDataRepository ForumUsers { get; }
        IUserDataRepository Users { get; }
        IForumCategoryRepository ForumCategory { get; }
        IForumBaseRepository ForumBase { get; }
        IForumTopicRepository ForumTopic { get; }
        IForumTopicCounterRepository ForumTopicCounter { get; }
        IForumPostRepository ForumPost { get; }
        IForumFileRepository ForumFile { get; }

        Task SaveAsync();
    }
}