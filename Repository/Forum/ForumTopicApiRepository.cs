using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using Entities.DTO.ForumDto;
using Entities.Models.Forum;
using Interfaces;
using Interfaces.Forum;
using Interfaces.Forum.API;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Repository.API.Forum
{
    public class ForumTopicApiRepository : RepositoryApi<ForumTopic, ILoggerManager, IHttpForumService>, IForumTopicApiRepository
    {
        public ForumTopicApiRepository(ILoggerManager logger, IHttpForumService httpClient) : base(logger, httpClient)
        {

        }
        public async Task<List<ForumViewTopicDto>> GetForumTopics(int categoryId, int forumId)
        {
            List<ForumViewTopicDto> forumViewTopicDtos = new List<ForumViewTopicDto>();


            var response = await _httpForumService.Client
                .GetAsync("api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumViewTopicDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewTopicDto>>(rawData).ToList();
            }
            else
            {
                _logger.LogError($"Unable to get topics for forum id: {forumId}");
            }

            return forumViewTopicDtos;
        }
        public async Task<List<ForumViewPostDto>> GetTopicPosts(int categoryId, int forumId, int topicId,
            int pageNumber, int pageSize, bool getAll = false)
        {
            List<ForumViewPostDto> forumViewPostDtos = new();

            bool allPosts = pageNumber == 0 && pageSize == 0;
            string uri = string.Empty;

            if (getAll)
            {
                uri = "api/categories/" + categoryId.ToString() +
                "/forums/" + forumId.ToString() +
                "/topics/" + topicId.ToString() + "/posts";
            }
            else
            {
                uri = "api/categories/" + categoryId.ToString() +
                "/forums/" + forumId.ToString() +
                "/topics/" + topicId.ToString() +
                "/posts?pageNumber=" + pageNumber.ToString() +
                "&pageSize=" + pageSize.ToString();
            }

            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(rawData);
                forumViewPostDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewPostDto>>(rawData).ToList();
            }
            else
            {
                _logger.LogError($"Unable to get posts for topic id: {topicId}");
            }

            return forumViewPostDtos;
        }
        public async Task<bool> UpdateTopicCounter(int categoryId, bool incresase, int postCountToDelete = 0)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri + "?&fields=TotalTopics");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalTopics = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData).First().TotalTopics;

                if (incresase)
                    totalTopics++;
                else
                {
                    if (postCountToDelete > 0)
                        totalTopics = -postCountToDelete;
                    else
                        totalTopics--;
                }

                var jsonPatchObject = new JsonPatchDocument<ForumViewCategoryDto>();
                jsonPatchObject.Replace(fc => fc.TotalTopics, totalTopics);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _httpForumService.Client.PatchAsync(uri,
                    new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to update topic counter for category id: {categoryId}");
                }
            }

            return result;
        }
        public async Task<int> GetTopicCount(int categoryId)
        {
            int totalTopics = 0;

            string uri = "api/categories/" + categoryId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri + "?&fields=TotalTopics");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                totalTopics = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData).First().TotalTopics;
                //totalPosts = int.Parse(JsonConvert.DeserializeObject<string>(rawData));
            }
            else
            {
                _logger.LogError($"Unable to get topic counter for category id: {categoryId}");
            }

            return totalTopics;
        }
        public async Task<bool> IncreaseViewCounterForTopic(int categoryId, int forumId, int topicId)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics/" + topicId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri + "?&fields=TotalViews");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalViews = JsonConvert.DeserializeObject<IEnumerable<ForumViewTopicDto>>(rawData).First().TotalViews;

                totalViews++;

                var jsonPatchObject = new JsonPatchDocument<ForumViewTopicDto>();
                jsonPatchObject.Replace(fc => fc.TotalViews, totalViews);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate =
                    await _httpForumService.Client
                    .PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to view counter for topic id: {topicId}");
                }
            }

            return result;
        }
        public async Task<int> CreateForumTopic(int categoryId, int forumId, ForumTopicForCreationDto topic)
        {
            bool result = false;
            int createdTopicId = 0;
            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics";

            var jsonContent = JsonConvert.SerializeObject(topic);

            var response = await _httpForumService.Client
                .PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
                var rawData = await response.Content.ReadAsStringAsync();
                createdTopicId = JsonConvert.DeserializeObject<ForumTopicDto>(rawData).Id;
            }
            else
            {
                _logger.LogError($"Unable create topic for forum id: {forumId}");
            }

            return createdTopicId;
        }
        public async Task<bool> DeleteForumTopic(int categoryId, int forumId, int topicId)
        {
            bool result = false;
            string uri = "api/categories/" +
                categoryId.ToString() + "/forums/" +
                forumId.ToString() + "/topics/" +
                topicId.ToString();

            var response = await _httpForumService.Client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable delete topic id: {topicId}");
            }

            return result;
        }
        public async Task<bool> CreateTopicPostCounter(int topicId, ForumCounterForCreationDto forumCounterForCreationDto)
        {
            bool result = false;
            string uri = "api" +
                "/tcounters/" + topicId.ToString();

            var jsonContent = JsonConvert.SerializeObject(forumCounterForCreationDto);

            var response = await _httpForumService.Client
                .PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable create topic counter for topic id: {topicId}");
            }

            return result;
        }
        public async Task<bool> DeleteTopicPostCounter(int topicId)
        {
            bool result = false;
            string uri = "api" +
                "/tcounters/" + topicId.ToString();

            var response = await _httpForumService.Client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable delete topic counter for topic id: {topicId}");
            }

            return result;
        }
    }
}