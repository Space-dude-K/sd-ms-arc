using AutoMapper;
using Interfaces;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.DTO.ForumDto;
using Entities.Models.Forum;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Entities.RequestFeatures.Forum;
using Newtonsoft.Json;
using Forum.Utility.ForumLinks;
using Forum.ModelBinders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using api_forum.ActionsFilters.Forum;
using api_forum.ActionsFilters;

namespace Forum.Controllers.Forum
{
    [Route("api/categories/{categoryId}/forums/{forumId}/topics/{topicId}/posts")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, User")]
    public class PostController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly PostLinks _postLinks;

        public PostController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, PostLinks postLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _postLinks = postLinks;
        }
        /// <summary>
        /// Gets allowed post options
        /// </summary>
        /// <returns>The options list</returns>
        /// <response code="200">Returns items</response>
        /// <response code="401">If unauthorized</response>
        [HttpOptions]
        public IActionResult GetPostOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
        /// <summary>
        /// Gets post count
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <returns>The posts list</returns>
        /// <response code="200">Returns items count</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If topic not found</response>
        [HttpGet("count", Name = "GetPostCountForTopic")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetPostCountForTopic(
            int categoryId, int forumId, int topicId, [FromQuery] ForumPostParameters forumPostParameters)
        {
            if (!forumPostParameters.ValidLikeRange)
                return BadRequest("Max likes cant be less than min");

            var topic = await _repository.ForumTopic.GetTopicAsync(forumId, topicId, trackChanges: false);

            if (topic == null)
            {
                _logger.LogInfo($"Topic with forum id: {forumId} and topic id: {topicId} doesn't exist in the database.");

                return NotFound();
            }

            var postsFromDb = await _repository.ForumPost.GetAllPostsFromTopicAsync(topicId, forumPostParameters, true, trackChanges: false);

            return Ok(postsFromDb.Count);
        }
        /// <summary>
        /// Gets the list of all posts
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <returns>The posts list</returns>
        /// <response code="200">Returns items</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If topic not found</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetPostsForTopic(
            int categoryId, int forumId, int topicId, [FromQuery] ForumPostParameters forumPostParameters)
        {
            if (!forumPostParameters.ValidLikeRange)
                return BadRequest("Max likes cant be less than min");

            var topic = await _repository.ForumTopic.GetTopicAsync(forumId, topicId, trackChanges: false);

            if (topic == null)
            {
                _logger.LogInfo($"Topic with forum id: {forumId} and topic id: {topicId} doesn't exist in the database.");

                return NotFound();
            }

            var postsFromDb = forumPostParameters.UserId > 0 ? 
                await _repository.ForumPost.GetAllPostsFromTopicAsyncFilteredByUserId(topicId, 
                forumPostParameters, false, trackChanges: false) :
                await _repository.ForumPost.GetAllPostsFromTopicAsync(topicId, forumPostParameters, 
                false, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(postsFromDb.MetaData));

            var postsDto = _mapper.Map<IEnumerable<ForumPostDto>>(postsFromDb);
            var links = _postLinks.TryGenerateLinks(postsDto, categoryId, forumId, topicId, forumPostParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        /// <summary>
        /// Gets the post
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <param name="postId"></param>
        /// <returns>The post</returns>
        /// <response code="200">Returns item</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If the topic or post doesn't exist</response>
        [HttpGet("{postId}", Name = "GetPostForTopic")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetPostForTopic(int categoryId, int forumId, int topicId, int postId, 
            [FromQuery] ForumPostParameters forumPostParameters)
        {
            var topicDb = await _repository.ForumTopic.GetTopicAsync(forumId, topicId, trackChanges: false);
            if (topicDb == null)
            {
                _logger.LogInfo($"Topic with forum id: {forumId} and post id: {postId} doesn't exist in the database.");

                return NotFound();
            }
            var postDb = await _repository.ForumPost.GetPostAsync(topicId, postId, trackChanges: false);
            if (postDb == null)
            {
                _logger.LogInfo($"Post with id: {postId} doesn't exist in the database.");

                return NotFound();
            }

            var postDto = _mapper.Map<ForumPostDto>(postDb);
            var links = _postLinks.TryGenerateLinks(new List<ForumPostDto>() { postDto }, 
                categoryId, forumId, topicId, forumPostParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        /// <summary>
        /// Gets the post collection
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <param name="ids"></param>
        /// <returns>The forum collection</returns>
        /// <response code="200">Returns items</response>
        /// <response code="400">If the id's is empty</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If some ids are not valid in a collection</response>
        [HttpGet("collection/({ids})", Name = "PostCollection")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetPostCollection(int categoryId, int forumId, int topicId, 
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids,
            [FromQuery] ForumPostParameters forumPostParameters)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var postEntities = await _repository.ForumPost.GetPostsFromTopicByIdsAsync(topicId, ids, trackChanges: false);

            if (ids.Count() != postEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var postsToReturn = _mapper.Map<IEnumerable<ForumPostDto>>(postEntities);
            var links = _postLinks
                .TryGenerateLinks(postsToReturn, categoryId, forumId, topicId, forumPostParameters.Fields, HttpContext, ids);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        /// <summary>
        /// Creates a newly created post
        /// </summary>
        /// <param name="post"></param>
        /// <returns>A newly created post</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePostForTopic(int categoryId, int forumId, int topicId, 
            [FromBody] ForumPostForCreationDto post)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ForumPostForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var topic = await _repository.ForumTopic.GetTopicAsync(forumId, topicId, trackChanges: false);
            if (topic == null)
            {
                _logger.LogInfo($"Topic with forum id: {forumId} and topic id: {topicId} doesn't exist in the database.");

                return NotFound();
            }

            var postEntity = _mapper.Map<ForumPost>(post);
            postEntity.CreatedAt = DateTime.Now;
 
            _repository.ForumPost.CreatePostForTopic(topicId, postEntity);
            await _repository.SaveAsync();
            var postToReturn = _mapper.Map<ForumPostDto>(postEntity);

            return CreatedAtRoute("GetPostForTopic", new { categoryId, forumId, topicId, postId = postToReturn.Id }, postToReturn);
        }
        /// <summary>
        /// Creates a newly created post collection
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <param name="postCollection"></param>
        /// <returns>A newly created post collection</returns>
        /// <response code="201">Returns the newly created items</response>
        /// <response code="400">If the items is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost("collection")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePostCollectionForForum(int categoryId, int forumId, int topicId, 
            [FromBody] IEnumerable<ForumPostForCreationDto> postCollection)
        {
            if (postCollection == null)
            {
                _logger.LogError("Post collection sent from client is null.");
                return BadRequest("Post collection is null");
            }

            var postEntities = _mapper.Map<IEnumerable<ForumPost>>(postCollection);

            foreach (var post in postEntities)
            {
                _repository.ForumPost.CreatePostForTopic(topicId, post);
            }

            await _repository.SaveAsync();
            var postCollectionToReturn = _mapper.Map<IEnumerable<ForumPostDto>>(postEntities);
            var ids = string.Join(",", postCollectionToReturn.Select(c => c.Id));

            return CreatedAtRoute("PostCollection", new { categoryId, forumId, topicId, ids }, postCollectionToReturn);
        }
        /// <summary>
        /// Deletes existing post
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <param name="postId"></param>
        /// <response code="204">If post is deleted</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If topic or post doesn't exist</response>
        [HttpDelete("{postId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidatePostForTopicExistsAttribute))]
        public async Task<IActionResult> DeletePostForTopic(int categoryId, int forumId, int topicId, int postId)
        {
            var postForTopic = HttpContext.Items["post"] as ForumPost;

            _repository.ForumPost.DeletePost(postForTopic);
            await _repository.SaveAsync();

            return NoContent();
        }
        /// <summary>
        /// Updates existing post
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <param name="postId"></param>
        /// <param name="post"></param>
        /// <response code="204">If post is updated</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPut("{postId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidatePostForTopicExistsAttribute))]
        public async Task<IActionResult> UpdatePostForTopic(int categoryId, int forumId, int topicId, int postId, 
            [FromBody] ForumPostForUpdateDto post)
        {
            var postForTopic = HttpContext.Items["post"] as ForumPost;

            _mapper.Map(post, postForTopic);
            await _repository.SaveAsync();

            return NoContent();
        }
        /// <summary>
        /// Partially updates existing post
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <param name="patchDoc"></param>
        /// <response code="204">If post is updated</response>
        /// <response code="400">If path doc is null</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPatch("{postId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidatePostForTopicExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdatePostForTopic(int categoryId, int forumId, int topicId, int postId,
            [FromBody] JsonPatchDocument<ForumPostForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var postEntity = HttpContext.Items["post"] as ForumPost;
            var postToPatch = _mapper.Map<ForumPostForUpdateDto>(postEntity);
            patchDoc.ApplyTo(postToPatch, ModelState);

            TryValidateModel(postToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(postToPatch, postEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}