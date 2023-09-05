using Forum.ViewModels;

namespace Interfaces.Forum
{
    public interface IHttpForumService
    {
        public HttpClient Client { get; }
    }
}