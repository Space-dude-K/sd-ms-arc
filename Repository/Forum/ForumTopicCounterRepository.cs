using Entities;
using Entities.Models.Forum;
using Entities.RequestFeatures;
using Interfaces.Forum;
using Microsoft.EntityFrameworkCore;

namespace Repository.Forum
{
    public class ForumTopicCounterRepository : RepositoryBase<ForumTopicCounter, ForumContext>, IForumTopicCounterRepository
    {
        public ForumTopicCounterRepository(ForumContext forumContext) : base(forumContext)
        {
        }
        public void DeleteTopicCounter(ForumTopicCounter forumTopicCounter)
        {
            Delete(forumTopicCounter);
        }
        public async Task<PagedList<ForumTopicCounter>> GetPostCountersAsync(bool trackChanges)
        {
            var topicCounters = await FindAll(trackChanges)
                .ToListAsync();

            return PagedList<ForumTopicCounter>.ToPagedList(topicCounters, 0, 0, true);
        }
        public async Task<ForumTopicCounter> GetPostCounterAsync(int? forumTopicId, bool trackChanges)
        {
            var topicCounter = await FindByCondition(f => f.ForumTopicId.Equals(forumTopicId), trackChanges)
                .SingleOrDefaultAsync();

            return topicCounter;
        }
        public void CreateTopicCounter(int topicId, ForumTopicCounter forumTopicCounter)
        {
            Create(forumTopicCounter);
        }
    }
}