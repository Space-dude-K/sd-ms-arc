using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Forum.ApiServices
{
    public interface IForumCategoryService
    {
        Task<bool> CreateForumCategory(ForumCategoryForCreationDto category);
        Task<List<ForumViewCategoryDto>> GetForumCategories();
        Task<bool> UpdateTotalPostCounterForCategory(int categoryId, bool incresase, int postCountToDelete = 0);
    }
}
