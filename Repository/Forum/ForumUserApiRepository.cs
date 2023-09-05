using Entities.DTO.UserDto;
using Entities.Models.Forum;
using Entities.ViewModels;
using Interfaces;
using Interfaces.Forum;
using Interfaces.Forum.API;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Text;

namespace Repository.API.Forum
{
    public class ForumUserApiRepository : RepositoryApi<ForumUser, ILoggerManager, IHttpForumService>, IForumUserApiRepository
    {
        public ForumUserApiRepository(ILoggerManager logger, IHttpForumService httpClient) : base(logger, httpClient)
        {

        }
        public async Task<ForumUserDto> GetForumUser(int userId)
        {
            ForumUserDto forumUser = new();

            string uri = "api/users/" + userId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumUser = JsonConvert.DeserializeObject<IEnumerable<ForumUserDto>>(rawData).First();
            }
            else
            {
                _logger.LogError($"Unable to get forum user id: {userId}");
            }

            return forumUser;
        }
        public async Task<List<string>> GetUserRoles()
        {
            var response = await _httpForumService.Client.GetAsync("api/roles");
            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<IdentityRole>>(rawData)
                .Select(r => r.Name).ToList();

            return responseContent;
        }
        // Only for testing
        public async Task<RegisterTableViewModel> GetUsersData()
        {
            var response = await _httpForumService.Client.GetAsync("api/users");
            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<ForumUserDto>>(rawData).ToList();

            var model = new RegisterTableViewModel()
            {
                AppUsers = responseContent
            };

            return model;
        }
        public async Task<ForumUserDto> GetForumUserDto(int userId)
        {
            ForumUserDto forumUserDto = new();

            string uri = "api/usersf/" + userId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumUserDto = JsonConvert.DeserializeObject<ForumUserDto>(rawData);
            }
            else
            {
                _logger.LogError($"Unable to get post counter for user id: {userId}");
            }

            return forumUserDto;
        }
        public async Task<bool> UpdateAppUser(int userId, AppUserDto appUserDto)
        {
            bool result = false;

            string uri = "api/usersa/" + userId.ToString();
            var response = await _httpForumService.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                var appUserFromDb = JsonConvert.DeserializeObject<AppUserDto>(rawData);
                appUserFromDb = appUserDto;

                var jsonAfterUpdade = JsonConvert.SerializeObject(appUserFromDb);
                var responseAfterUpdate =
                    await _httpForumService.Client.PutAsync(uri, new StringContent(jsonAfterUpdade, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
            }
            else
            {
                _logger.LogError($"Unable update app user id: {userId}");
            }

            return result;
        }
    }
}