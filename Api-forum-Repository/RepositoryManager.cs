using Interfaces.Forum;
using Interfaces;
using Repository.API.Forum;
using Interfaces.Forum.API;
using Entities;
using Repository.Forum;
using Repository.User;
using Interfaces.File;
using Interfaces.User;
using Repository.File;
using Microsoft.Extensions.Logging;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ILogger<RepositoryManager> _logger;
        private readonly ForumContext _forumContext;

        private IRoleRepository _roleRepository;
        private UserDataRepository _userDataRepository;
        private ForumUserDataRepository _forumUserDataRepository;

        private IForumCategoryRepository _forumCategoryRepository;
        private IForumBaseRepository _forumBaseRepository;
        private IForumTopicRepository _forumTopicRepository;
        private IForumTopicCounterRepository _forumTopicCounterRepository;
        private IForumPostRepository _forumPostRepository;

        private IForumFileRepository _forumFileRepository;

        private IForumUserRepository _forumUser;

        public RepositoryManager(ILogger<RepositoryManager> logger, ForumContext forumContext)
        {
            _logger = logger;
            _forumContext = forumContext;
        }
        public IRoleRepository Roles
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new RoleRepository(_forumContext);

                return _roleRepository;
            }
        }
        public IForumUserDataRepository ForumUsers
        {
            get
            {
                if (_forumUserDataRepository == null)
                    _forumUserDataRepository = new ForumUserDataRepository(_forumContext);

                return _forumUserDataRepository;
            }
        }
        public IUserDataRepository Users
        {
            get
            {
                if (_userDataRepository == null)
                    _userDataRepository = new UserDataRepository(_forumContext);

                return _userDataRepository;
            }
        }

        public IForumCategoryRepository ForumCategory
        {
            get
            {
                if (_forumCategoryRepository == null)
                    _forumCategoryRepository = new ForumCategoryRepository(_forumContext);
                return _forumCategoryRepository;
            }
        }

        public IForumBaseRepository ForumBase
        {
            get
            {
                if (_forumBaseRepository == null)
                    _forumBaseRepository = new ForumBaseRepository(_forumContext);
                return _forumBaseRepository;
            }
        }
        //
        public IForumTopicRepository ForumTopic
        {
            get
            {
                if (_forumTopicRepository == null)
                    _forumTopicRepository = new ForumTopicRepository(_forumContext);
                return _forumTopicRepository;
            }
        }
        public IForumTopicCounterRepository ForumTopicCounter
        {
            get
            {
                if (_forumTopicCounterRepository == null)
                    _forumTopicCounterRepository = new ForumTopicCounterRepository(_forumContext);
                return _forumTopicCounterRepository;
            }
        }
        public IForumPostRepository ForumPost
        {
            get
            {
                if (_forumPostRepository == null)
                    _forumPostRepository = new ForumPostRepository(_forumContext);
                return _forumPostRepository;
            }
        }
        public IForumFileRepository ForumFile
        {
            get
            {
                if (_forumFileRepository == null)
                    _forumFileRepository = new ForumFileRepository(_forumContext);
                return _forumFileRepository;
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