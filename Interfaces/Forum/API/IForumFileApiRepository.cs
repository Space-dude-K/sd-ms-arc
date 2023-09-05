using Entities.DTO.FileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Forum.API
{
    public interface IForumFileApiRepository
    {
        Task<bool> CreateForumFile(ForumFileDto file);
        Task<ForumFileDto> GetForumFileByUserId(int forumUserId);
        Task<List<ForumFileDto>> GetForumFilesByUserAndPostId(int forumUserId, int postId);
        Task<bool> UpdateForumFile(int forumUserId, ForumFileDto forumFileDto);
    }
}
