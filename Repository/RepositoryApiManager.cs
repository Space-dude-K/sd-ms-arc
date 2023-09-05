using Interfaces.Forum;
using Interfaces;
using Repository.API.Forum;
using Interfaces.Forum.API;
using Repository.API.File;

namespace Repository
{
    public class RepositoryApiManager : IRepositoryApiManager
    {
        private readonly ILoggerManager _logger;
        private readonly IHttpForumService _httpForumService;

        private IForumCategoryApiRepository _categoryApis;
        private IForumBaseApiRepository _forumApis;
        private IForumTopicApiRepository _topicApis;
        private IForumPostApiRepository _postApis;

        private IForumFileApiRepository _fileApis;

        private IForumUserApiRepository _forumUserApis;

        public RepositoryApiManager(ILoggerManager logger, IHttpForumService httpForumService)
        {
            _logger = logger;
            _httpForumService = httpForumService;
        }
        public IForumCategoryApiRepository CategoryApis
        {
            get
            {
                if (_categoryApis == null)
                    _categoryApis = new ForumCategoryApiRepository(_logger, _httpForumService);

                return _categoryApis;
            }
        }
        public IForumBaseApiRepository ForumApis
        {
            get
            {
                if (_forumApis == null)
                    _forumApis = new ForumBaseApiRepository(_logger, _httpForumService);

                return _forumApis;
            }
        }
        public IForumTopicApiRepository TopicApis
        {
            get
            {
                if (_topicApis == null)
                    _topicApis = new ForumTopicApiRepository(_logger, _httpForumService);

                return _topicApis;
            }
        }
        public IForumPostApiRepository PostApis
        {
            get
            {
                if (_postApis == null)
                    _postApis = new ForumPostApiRepository(_logger, _httpForumService);

                return _postApis;
            }
        }
        public IForumFileApiRepository FileApis
        {
            get
            {
                if (_fileApis == null)
                    _fileApis = new ForumFileApiRepository(_logger, _httpForumService);

                return _fileApis;
            }
        }
        public IForumUserApiRepository ForumUserApis
        {
            get
            {
                if (_forumUserApis == null)
                    _forumUserApis = new ForumUserApiRepository(_logger, _httpForumService);

                return _forumUserApis;
            }
        }
    }
}