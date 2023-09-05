using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto;
using Entities.DTO.UserDto;
using Entities.Models.Forum;
using Interfaces;
using Interfaces.Forum;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Text;
using Interfaces.Forum.API;
using Entities.DTO.ForumDto.Update;

namespace Repository.API.Forum
{
    public class ForumPostApiRepository : RepositoryApi<ForumPost, ILoggerManager, IHttpForumService>, IForumPostApiRepository
    {
        public ForumPostApiRepository(ILoggerManager logger, IHttpForumService httpClient) : base(logger, httpClient)
        {

        }
        public async Task<int> GetTopicPostCount(int topicId)
        {
            int totalPosts = 0;

            string uri = "api" +
                "/tcounters/" + topicId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {

                var rawData = await response.Content.ReadAsStringAsync();
                totalPosts = JsonConvert.DeserializeObject<ForumTopicCounterDto>(rawData).PostCounter;
            }
            else
            {
                _logger.LogError($"Unable to get topic post counter for topic id: {topicId}");
            }

            return totalPosts;
        }
        public async Task<bool> UpdatePost(int categoryId, int forumId, int topicId, int postId, 
            ForumPostForUpdateDto newPostDto)
        {
            bool result = false;

            string uri = "api/categories/" +
                categoryId.ToString() + "/forums/" +
                forumId.ToString() + "/topics/" +
                topicId.ToString() + "/posts/" +
                postId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                var postDto = JsonConvert.DeserializeObject<IEnumerable<ForumPostDto>>(rawData).First();

                postDto.PostText = newPostDto.PostText;
                postDto.UpdatedAt = DateTime.Now;
                postDto.Likes = newPostDto.Likes;
                //postDto.ForumUserId = 1;

                var jsonAfterUpdade = JsonConvert.SerializeObject(postDto);
                var responseAfterUpdate =
                    await _httpForumService.Client
                    .PutAsync(uri, new StringContent(jsonAfterUpdade, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
            }
            else
            {
                _logger.LogError($"Unable update post with id: {postId}");
            }

            return result;
        }
        public async Task<bool> DeleteForumPost(int categoryId, int forumId, int topicId, int postId)
        {
            bool result = false;
            string uri = "api/categories/" +
                categoryId.ToString() + "/forums/" +
                forumId.ToString() + "/topics/" +
                topicId.ToString() + "/posts/" +
                postId.ToString();

            var response = await _httpForumService.Client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable delete post for topic id: {topicId}");
            }

            return result;
        }
        public async Task<int> CreateForumPost(int categoryId, int forumId, int topicId, ForumPostForCreationDto post)
        {
            int createdPostId = 0;
            string uri = "api/categories/" 
                + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics/" + topicId.ToString() + "/posts";

            var jsonContent = JsonConvert.SerializeObject(post);

            var response = await _httpForumService.Client
                .PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                var createdPostDto = JsonConvert.DeserializeObject<ForumPostDto>(rawData);
                createdPostId = createdPostDto.Id;
                _logger.LogInfo($"Created post id: {createdPostId}");
            }
            else
            {
                _logger.LogError($"Unable create post for topic id: {topicId}");
            }

            return createdPostId;
        }
        public async Task<bool> UpdatePostCounter(int topicId, bool incresase, int postCountToDelete = 0)
        {
            bool result = false;

            string uri = "api" +
                "/tcounters/" + topicId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();

                // Nothing to update. Post deleted.
                if (string.IsNullOrEmpty(rawData))
                    return true;

                int totalPosts = JsonConvert.DeserializeObject<ForumTopicCounterDto>(rawData).PostCounter;

                if (incresase)
                    totalPosts++;
                else
                {
                    if (postCountToDelete > 0)
                        totalPosts = -postCountToDelete;
                    else
                        totalPosts--;
                }

                JsonPatchDocument<ForumTopicCounterDto> jsonPatchObject = new();
                jsonPatchObject.Replace(fc => fc.PostCounter, totalPosts);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _httpForumService.Client
                    .PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to update post counter for topic id: {topicId}");
                }
            }

            return result;
        }
        public async Task<bool> UpdatePostCounterForUser(int userId, bool incresase, int postCountToDelete = 0)
        {
            bool result = false;

            string uri = "api/usersf/" + userId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalPosts = JsonConvert.DeserializeObject<ForumUserDto>(rawData).TotalPostCounter;

                if (incresase)
                    totalPosts++;
                else
                {
                    if (postCountToDelete > 0)
                        totalPosts -= postCountToDelete;
                    else
                        totalPosts--;
                }

                JsonPatchDocument<ForumUserDto> jsonPatchObject = new();
                jsonPatchObject.Replace(fc => fc.TotalPostCounter, totalPosts);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _httpForumService.Client
                    .PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to update post counter for user id: {userId}");
                }
            }

            return result;
        }
        public async Task<bool> UpdatePostLikeCounter(int categoryId, int forumId, int topicId, int postId, bool incresase)
        {
            bool result = false;

            string uri = "api/categories/" +
                categoryId.ToString() + "/forums/" +
                forumId.ToString() + "/topics/" +
                topicId.ToString() + "/posts/" +
                postId.ToString();

            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();

                // Nothing to update. Post deleted.
                if (string.IsNullOrEmpty(rawData))
                    return true;

                int totalLikes = JsonConvert.DeserializeObject<ICollection<ForumPostDto>>(rawData).First().Likes;

                if (incresase)
                    totalLikes++;
                else
                {
                    totalLikes--;
                }

                JsonPatchDocument<ForumPostForUpdateDto> jsonPatchObject = new();
                jsonPatchObject.Replace(fc => fc.Likes, totalLikes);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _httpForumService.Client
                    .PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to update like counter for topic id: {postId}");
                }
            }

            return result;
        }
    }
}