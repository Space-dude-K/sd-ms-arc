using AutoMapper;
using Forum.Utility.UserLinks;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Entities.DTO.FileDto;
using Entities.Models.File;
using Entities.DTO.FileDto.Update;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Entities.DTO.ForumDto;
using Entities.RequestFeatures.Forum;
using Forum.Utility.ForumLinks;
using Forum.ModelBinders;
using api_forum.ActionsFilters.File;
using api_forum.ActionsFilters;
using Repository;

namespace Forum.ApiControllers.File
{
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, User")]
    public class FileController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        //private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserDataLinks _userDataLinks;

        public FileController(IRepositoryManager repository, 
            ILoggerManager logger, IMapper mapper, UserDataLinks userDataLinks)
        {
            _repository = repository;
            //_logger = logger;
            _mapper = mapper;
            _userDataLinks = userDataLinks;
        }
        /// <summary>
        /// Gets the file for user avatar
        /// </summary>
        /// <param name="forumUserId"></param>
        /// <returns>The file</returns>
        /// <response code="200">User avatar image</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="400">If forum user not found</response>
        /// /// <response code="401">If file doesn't exist</response>
        [HttpGet("file/{forumUserId}", Name = "GetForumFile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetForumFileByUserId(int forumUserId)
        {
            if (forumUserId == 0)
                return BadRequest("Bad request. Missing forum user id.");

            var file = await _repository.ForumFile.GetFileAsync(forumUserId, trackChanges: false);

            if (file == null)
            {
                //_logger.LogInfo($"File with user id: {forumUserId} doesn't exist in the database.");
                return NotFound();
            }

            return Ok(file);
        }
        /// <summary>
        /// Gets the file for user avatar
        /// </summary>
        /// <param name="forumUserId"></param>
        /// <param name="postId"></param>
        /// <returns>The file</returns>
        /// <response code="200">User avatar image</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="400">If forum user not found</response>
        /// /// <response code="401">If file doesn't exist</response>
        [HttpGet("file/{forumUserId}/{postId}", Name = "GetForumFileByUserAndPostId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetForumFileByUserAndPostId(int forumUserId, int postId)
        {
            if (forumUserId == 0)
                return BadRequest("Bad request. Missing forum user id.");

            var files = await _repository.ForumFile.GetFilesAsync(forumUserId, postId, trackChanges: false);

            /*if (file == null)
            {
                //_logger.LogInfo($"File with user id: {forumUserId} doesn't exist in the database.");
                return NotFound();
            }*/

            return Ok(files);
        }
        /// <summary>
        /// Writes the file data for the user avatar
        /// </summary>
        /// <param name="file"></param>
        /// <response code="200">Successfully uploaded</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost("file", Name = "CreateForumFile")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateForumFile([FromBody] ForumFileDto file)
        {
            if (!ModelState.IsValid)
            {
                //_logger.LogError("Invalid model state for the ForumFileDto object");
                return UnprocessableEntity(ModelState);
            }

            var fileEntity = _mapper.Map<ForumFile>(file);
            _repository.ForumFile.CreateFile(fileEntity);
            await _repository.SaveAsync();

            return Ok();
        }
        /// <summary>
        /// Updates the file data for the user avatar
        /// </summary>
        /// <param name="forumUserId"></param>
        /// <param name="fileDto"></param>
        /// <response code="200">Successfully updated</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPut("file/{forumUserId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateFileExistsAttribute))]
        public async Task<IActionResult> UpdateForumFile(int forumUserId, [FromBody] ForumFileForUpdateDto fileDto)
        {
            if (!ModelState.IsValid)
            {
                //_logger.LogError("Invalid model state for the ForumFileForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }

            var file = HttpContext.Items["file"] as ForumFile;
            _mapper.Map(fileDto, file);
            await _repository.SaveAsync();

            return Ok();
        }
    }
}