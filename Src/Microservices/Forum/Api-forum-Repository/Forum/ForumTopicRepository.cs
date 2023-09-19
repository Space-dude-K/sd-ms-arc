using Entities;
using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore;
using Entities.RequestFeatures.Forum;
using Entities.RequestFeatures;
using Repository.Extensions;
using Interfaces.Forum.API;

namespace Repository.Forum
{
    public class ForumTopicRepository : RepositoryBase<ForumTopic, ForumContext>, IForumTopicRepository
    {
        public ForumTopicRepository(ForumContext forumContext) : base(forumContext)
        {
        }
        public void CreateTopicForForum(int forumBaseId, ForumTopic topic)
        {
            Create(topic);
        }
        public void DeleteTopic(ForumTopic topic)
        {
            Delete(topic);
        }
        public async Task<PagedList<ForumTopic>> GetAllTopicsFromForumAsync(
            int? forumBaseId, ForumTopicParameters forumTopicParameters, bool trackChanges)
        {
            var topics = await FindByCondition(f => f.ForumBaseId.Equals(forumBaseId), trackChanges)
                .Search(forumTopicParameters.SearchTerm)
                .Sort(forumTopicParameters.OrderBy)
                .ToListAsync();

            return PagedList<ForumTopic>.ToPagedList(topics, forumTopicParameters.PageNumber, forumTopicParameters.PageSize);
        }
        public async Task<ForumTopic> GetTopicAsync(int forumBaseId, int topicId, bool trackChanges)
        {
            return await FindByCondition(c => c.ForumBaseId.Equals(forumBaseId) && c.Id.Equals(topicId), trackChanges)
                .SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<ForumTopic>> GetTopicsFromForumByIdsAsync(int forumId, 
            IEnumerable<int> ids, bool trackChanges)
        {
            return await FindByCondition(t => t.ForumBaseId.Equals(forumId) && ids.Contains(t.Id), trackChanges).ToListAsync();
        }
    }
}