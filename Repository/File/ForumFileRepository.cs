using Entities;
using Entities.Models.File;
using Interfaces.File;
using Microsoft.EntityFrameworkCore;

namespace Repository.File
{
    public class ForumFileRepository : RepositoryBase<ForumFile, ForumContext>, IForumFileRepository
    {
        public ForumFileRepository(ForumContext forumContext) : base(forumContext)
        {
        }
        public async Task<ForumFile> GetFileAsync(int forumUserId, bool trackChanges)
        {
            return await FindByCondition(c => c.ForumUserId.Equals(forumUserId), trackChanges)
                .SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<ForumFile>> GetFilesAsync(int forumUserId, int postId, bool trackChanges)
        {
            return await FindByCondition(c => c.ForumUserId.Equals(forumUserId) 
            && c.ForumPostId.Equals(postId), trackChanges)
                .ToListAsync();
        }
        public async Task<ForumFile> GetFileAsyncById(int id, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(id), trackChanges)
                .SingleOrDefaultAsync();
        }
        public void CreateFile(ForumFile file)
        {
            Create(file);
        }
    }
}