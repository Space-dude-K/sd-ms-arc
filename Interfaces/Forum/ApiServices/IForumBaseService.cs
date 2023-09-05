using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Forum.ApiServices
{
    public interface IForumBaseService
    {
        Task<bool> CreateForumBase(int categoryId, ForumBaseForCreationDto forum);
        Task<bool> DeleteForumBase(int categoryId, int forumId);
        Task<ForumViewBaseDto> GetForumBase(int categoryId, int forumBaseId);
        Task<List<ForumViewBaseDto>> GetForumBases(int categoryId);
        Task<bool> IncreaseViewCounterForForumBase(int categoryId, int forumId);
    }
}
