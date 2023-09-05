using Entities.DTO.FileDto;
using Entities.Models.File;
using Interfaces;
using Interfaces.Forum;
using Interfaces.Forum.API;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace Repository.API.File
{
    public class ForumFileApiRepository : RepositoryApi<ForumFile, ILoggerManager, IHttpForumService>, IForumFileApiRepository
    {
        public ForumFileApiRepository(ILoggerManager logger, IHttpForumService httpClient) : base(logger, httpClient)
        {

        }
        public async Task<List<ForumFileDto>> GetForumFilesByUserAndPostId(int forumUserId, int postId)
        {
            List<ForumFileDto> forumFilesDtoFromDb = new();

            string uri = "api/file/" + forumUserId.ToString() + "/" + postId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumFilesDtoFromDb = JsonConvert.DeserializeObject<IEnumerable<ForumFileDto>>(rawData).ToList();
            }
            else
            {
                _logger.LogError($"Missing forum files for user id: {forumUserId} and postId: {postId}");
            }

            return forumFilesDtoFromDb;
        }
        public async Task<ForumFileDto> GetForumFileByUserId(int forumUserId)
        {
            ForumFileDto forumFileDtoFromDb = new();

            string uri = "api/file/" + forumUserId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumFileDtoFromDb = JsonConvert.DeserializeObject<ForumFileDto>(rawData);
            }
            else
            {
                _logger.LogError($"Missing forum file for user id: {forumUserId}");
            }

            return forumFileDtoFromDb;
        }
        public async Task<bool> CreateForumFile(ForumFileDto file)
        {
            bool result = false;
            string uri = "api/file";

            var jsonContent = JsonConvert.SerializeObject(file);

            var response = await _httpForumService.Client
                .PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable create file for user id: {file.ForumUserId}");
            }

            return result;
        }
        public async Task<bool> UpdateForumFile(int forumUserId, ForumFileDto forumFileDto)
        {
            bool result = false;

            string uri = "api/file/" + forumUserId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                var forumFileDtoFromDb = JsonConvert.DeserializeObject<ForumFileDto>(rawData);
                forumFileDtoFromDb.Path = forumFileDto.Path;
                forumFileDtoFromDb.Name = forumFileDto.Name;

                var jsonAfterUpdade = JsonConvert.SerializeObject(forumFileDtoFromDb);
                var responseAfterUpdate =
                    await _httpForumService.Client.PutAsync(uri, new StringContent(jsonAfterUpdade, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
            }
            else
            {
                _logger.LogError($"Unable update file with user id: {forumUserId}");
            }

            return result;
        }
    }
}