using api_forum.ActionsFilters;
using api_forum.ActionsFilters.User;
using AutoMapper;
using Entities.DTO.ForumDto.Update;
using Entities.DTO.UserDto;
using Entities.DTO.UserDto.Update;
using Entities.Models;
using Entities.Models.Forum;
using Entities.RequestFeatures.User;
using Forum.Utility.UserLinks;
using Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Forum.ApiControllers.User
{
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserDataLinks _userDataLinks;

        public UserController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, UserDataLinks userDataLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userDataLinks = userDataLinks;
        }
        [HttpOptions]
        public IActionResult GetUserOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS");
            return Ok();
        }
        [HttpGet("roles", Name = "GetUserRoles")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<List<AppRole>> GetUserRoles()
        {
            var rolesFromDb = await _repository.Roles.GetAllRolesAsync(trackChanges: false);

            return rolesFromDb;
        }
        [HttpGet("users", Name = "GetUsers")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetUsers([FromQuery] UserParameters userParameters)
        {
            var usersFromDb = await _repository.Users.GetAllUsersAsync(userParameters, trackChanges: false);
            var usersDto = _mapper.Map<IEnumerable<ForumUserDto>>(usersFromDb);
            var links = _userDataLinks.TryGenerateLinks(usersDto, userParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        [HttpGet("users/{userId}", Name = "GetUserById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetUser(string userId, [FromQuery] UserParameters userParameters)
        {
            var usersFromDb = await _repository.Users.GetUserAsync(userId, userParameters, trackChanges: false);
            var usersDto = _mapper.Map<ForumUserDto>(usersFromDb);
            var links = _userDataLinks.TryGenerateLinks(new List<ForumUserDto>() { usersDto }, userParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        [HttpGet("usersf/{userId}", Name = "GetForumUserById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetForumUser(int userId)
        {
            var usersFromDb = await _repository.ForumUsers.GetUserAsync(userId, trackChanges: false);
            var usersDto = _mapper.Map<ForumUserDto>(usersFromDb);

            return Ok(usersDto);
        }
        [HttpGet("usersa/{userId}")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetAppUser(int userId)
        {
            var usersFromDb = await _repository.Users.GetUserAsync(userId, trackChanges: false);
            var usersDto = _mapper.Map<AppUser>(usersFromDb);

            return Ok(usersDto);
        }
        [HttpPut]
        [Route("usersa/{userId}")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [ServiceFilter(typeof(ValidateAppUserExistsAttribute))]
        public async Task<IActionResult> UpdateAppUser(int userId, [FromBody] AppUserForUpdateDto appUser)
        {
            var appUserFromDb = HttpContext.Items["user"] as AppUser;

            _mapper.Map(appUser, appUserFromDb);
            await _repository.SaveAsync();

            return NoContent();
        }
        [HttpPatch]
        [Route("usersf/{userId}")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [ServiceFilter(typeof(ValidateForumUserExistsAttribute))]
        public async Task<IActionResult> PatchUser(int userId, [FromBody] JsonPatchDocument<ForumUserForUpdateDto> userDoc)
        {
            if (userDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var userEntity = HttpContext.Items["user"] as ForumUser;
            var userToPatch = _mapper.Map<ForumUserForUpdateDto>(userEntity);
            userDoc.ApplyTo(userToPatch, ModelState);

            TryValidateModel(userToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(userToPatch, userEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}