using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using Entities.DTO.ForumDto;
using Entities.Models.Forum;
using Interfaces;
using Interfaces.Forum;
using Interfaces.Forum.API;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Text;

namespace Repository.API.Forum
{
    public class ForumCategoryApiRepository : RepositoryApi<ForumCategory, ILoggerManager, IHttpForumService>, IForumCategoryApiRepository
    {
        public ForumCategoryApiRepository(ILoggerManager logger, IHttpForumService httpClient) : base(logger, httpClient)
        {

        }
        public async Task<List<ForumViewCategoryDto>> GetForumCategories()
        {
            List<ForumViewCategoryDto> forumViewCategoryDtos = new();

            var response = await _httpForumService.Client.GetAsync("api/categories");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumViewCategoryDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewCategoryDto>>(rawData).ToList();
            }
            else
            {
                _logger.LogError($"Unable to get categories");
            }

            return forumViewCategoryDtos;
        }
        public async Task<bool> CreateForumCategory(ForumCategoryForCreationDto category)
        {
            bool result = false;
            string uri = "api/categories/";

            var jsonContent = JsonConvert.SerializeObject(category);

            var response = await _httpForumService.Client
                .PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable create category for category id: {category.Name}");
            }

            return result;
        }
        public async Task<bool> UpdateTotalPostCounterForCategory(int categoryId, bool incresase, int postCountToDelete = 0)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalPosts = JsonConvert.DeserializeObject<ICollection<ForumCategoryDto>>(rawData).First().TotalPosts;

                if (incresase)
                    totalPosts++;
                else
                {
                    if (postCountToDelete > 0)
                        totalPosts = -postCountToDelete;
                    else
                        totalPosts--;
                }

                JsonPatchDocument<ForumCategoryDto> jsonPatchObject = new();
                jsonPatchObject.Replace(fc => fc.TotalPosts, totalPosts);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _httpForumService.Client.PatchAsync(uri,
                    new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to update total post counter for category id: {categoryId}");
                }
            }

            return result;
        }
    }
}