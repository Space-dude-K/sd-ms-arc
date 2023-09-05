using Entities.Models.File;

namespace Interfaces.File
{
    public interface IForumFileRepository
    {
        void CreateFile(ForumFile file);
        Task<ForumFile> GetFileAsync(int forumUserId, bool trackChanges);
        Task<ForumFile> GetFileAsyncById(int id, bool trackChanges);
        Task<IEnumerable<ForumFile>> GetFilesAsync(int forumUserId, int postId, bool trackChanges);
    }
}
