using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using Entities.Models.Forum;
using Interfaces;
using Interfaces.Forum;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Repository.API.Forum
{
    public class ForumBaseApiRepository : RepositoryApi<ForumBase, ILoggerManager, IHttpForumService>, IForumBaseApiRepository
    {
        public ForumBaseApiRepository(ILoggerManager logger, IHttpForumService httpClient) : base(logger, httpClient)
        {

        }
        public async Task<bool> CreateForumBase(int categoryId, ForumBaseForCreationDto forum)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString() + "/forums";

            var jsonContent = JsonConvert.SerializeObject(forum);

            var response = await _httpForumService.Client
                .PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable create forum for category id: {categoryId}");
            }

            return result;
        }
        public async Task<bool> IncreaseViewCounterForForumBase(int categoryId, int forumId)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri + "?&fields=TotalViews");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalViews = JsonConvert
                    .DeserializeObject<IEnumerable<ForumViewBaseDto>>(rawData).First().TotalViews;

                totalViews++;

                var jsonPatchObject = new JsonPatchDocument<ForumViewBaseDto>();
                jsonPatchObject.Replace(fc => fc.TotalViews, totalViews);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _httpForumService.Client
                    .PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to view counter for category id: {categoryId}");
                }
            }

            return result;
        }
        public async Task<ForumViewBaseDto> GetForumBase(int categoryId, int forumBaseId)
        {
            ForumViewBaseDto forumViewBaseDto = new();

            var response = await _httpForumService.Client
                .GetAsync("api/categories/" + categoryId.ToString()
                + "/forums/" + forumBaseId.ToString());

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumViewBaseDto = JsonConvert
                    .DeserializeObject<IEnumerable<ForumViewBaseDto>>(rawData).First();
            }
            else
            {
                _logger.LogError($"Unable to get forums for category id: {categoryId}");
            }

            return forumViewBaseDto;
        }
        public async Task<List<ForumViewBaseDto>> GetForumBases(int categoryId)
        {
            List<ForumViewBaseDto> forumViewBaseDtos = new();

            var response = await _httpForumService.Client
                .GetAsync("api/categories/" + categoryId.ToString() + "/forums");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumViewBaseDtos = JsonConvert
                    .DeserializeObject<IEnumerable<ForumViewBaseDto>>(rawData).ToList();
            }
            else
            {
                _logger.LogError($"Unable to get forums for category id: {categoryId}");
            }

            return forumViewBaseDtos;
        }
        public async Task<bool> DeleteForumBase(int categoryId, int forumId)
        {
            bool result = false;
            string uri = "api/categories/" +
                categoryId.ToString() + "/forums/" +
                forumId.ToString();

            var response = await _httpForumService.Client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable delete forum base id: {forumId}");
            }

            return result;
        }
    }
}