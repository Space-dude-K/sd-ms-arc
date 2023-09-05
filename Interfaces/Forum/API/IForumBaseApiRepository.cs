using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;

namespace Repository.API.Forum
{
    public interface IForumBaseApiRepository
    {
        Task<bool> CreateForumBase(int categoryId, ForumBaseForCreationDto forum);
        Task<bool> DeleteForumBase(int categoryId, int forumId);
        Task<ForumViewBaseDto> GetForumBase(int categoryId, int forumBaseId);
        Task<List<ForumViewBaseDto>> GetForumBases(int categoryId);
        Task<bool> IncreaseViewCounterForForumBase(int categoryId, int forumId);
    }
}