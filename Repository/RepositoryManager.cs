using Interfaces.Forum;
using Interfaces;
using Repository.API.Forum;
using Interfaces.Forum.API;
using Repository.API.File;
using Entities;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ILoggerManager _logger;
        private readonly ForumContext _forumContext;

        private IForumCategoryRepository _category;
        private IForumBaseRepository _forum;
        private IForumTopicRepository _topic;
        private IForumPostRepository _post;

        private IForumFileRepository _file;

        private IForumUserRepository _forumUser;

        public RepositoryManager(ILoggerManager logger, ForumContext forumContext)
        {
            _logger = logger;
        }
        public IForumCategoryRepository ForumCategory
        {
            get
            {
                if (_category == null)
                    _category = new ForumCategoryRepository(_logger, _forumContext);

                return _category;
            }
        }
        public IForumBaseRepository ForumBase
        {
            get
            {
                if (_forum == null)
                    _forum = new ForumBaseRepository(_logger, _forumContext);

                return _forum;
            }
        }
        public IForumTopicRepository ForumTopic
        {
            get
            {
                if (_topic == null)
                    _topic = new ForumTopicRepository(_logger, _forumContext);

                return _topic;
            }
        }
        public IForumPostRepository ForumPost
        {
            get
            {
                if (_post == null)
                    _post = new ForumPostRepository(_logger, _forumContext);

                return _post;
            }
        }
        public IForumFileRepository ForumFile
        {
            get
            {
                if (_file == null)
                    _file = new ForumFileRepository(_logger, _forumContext);

                return _file;
            }
        }
        public IForumUserRepository ForumUser
        {
            get
            {
                if (_forumUser == null)
                    _forumUser = new ForumUserRepository(_logger, _forumContext);

                return _forumUser;
            }
        }
        public Task SaveAsync()
        {
            if (_forumContext.ChangeTracker.HasChanges())
                return _forumContext.SaveChangesAsync();

            return Task.CompletedTask;
        }
    }
}