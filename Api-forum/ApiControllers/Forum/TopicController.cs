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
    [Route("api/categories/{categoryId}/forums/{forumId}/topics")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, User")]
    public class TopicController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly TopicLinks _topicLinks;

        public TopicController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, TopicLinks topicLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _topicLinks = topicLinks;
        }
        /// <summary>
        /// Gets allowed topic options
        /// </summary>
        /// <returns>The options list</returns>
        /// <response code="200">Returns items</response>
        /// <response code="401">If unauthorized</response>
        [HttpOptions]
        public IActionResult GetTopicsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
        /// <summary>
        /// Gets the list of all topics
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <returns>The topics list</returns>
        /// <response code="200">Returns items</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If forum not found</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetTopicsForForum(
            int categoryId, int forumId, [FromQuery] ForumTopicParameters forumTopicParameters)
        {
            var forum = await _repository.ForumBase.GetForumFromCategoryAsync(categoryId, forumId, trackChanges: false);

            if (forum == null)
            {
                _logger.LogInfo($"Forum with category id: {categoryId} and forum id: {forumId} doesn't exist in the database.");
                return NotFound();
            }

            var topicsFromDb = await _repository.ForumTopic.GetAllTopicsFromForumAsync(forumId, forumTopicParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(topicsFromDb.MetaData));

            var topicsDto = _mapper.Map<IEnumerable<ForumTopicDto>>(topicsFromDb);
            var links = _topicLinks.TryGenerateLinks(topicsDto, categoryId, forumId, forumTopicParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        /// <summary>
        /// Creates topic counter
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="topicCounterDto"></param>
        /// <returns>The topics list</returns>
        /// <response code="200">Returns created counter</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If forum not found</response>
        [HttpPost]
        [Route("/api/tcounters/{topicId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTopicCounter(int topicId, [FromBody] ForumCounterForCreationDto topicCounterDto)
        {
            var topicCounterEntity = _mapper.Map<ForumTopicCounter>(topicCounterDto);
            _repository.ForumTopicCounter.CreateTopicCounter(topicId, topicCounterEntity);
            await _repository.SaveAsync();

            return Ok(topicCounterEntity);
        }
        /// <summary>
        /// Deletes topic counter
        /// </summary>
        /// <param name="topicCounterId"></param>
        /// <param name="topicCounterDto"></param>
        [HttpDelete]
        [Route("/api/tcounters/{topicId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidateTopicCounter))]
        public async Task<IActionResult> DeleteTopicCounter(int topicId)
        {
            var topicCounter = HttpContext.Items["topicCounter"] as ForumTopicCounter;

            _repository.ForumTopicCounter.DeleteTopicCounter(topicCounter);
            await _repository.SaveAsync();

            return NoContent();
        }
        /// <summary>
        /// Gets counters for topic
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>The post counter</returns>
        /// <response code="200">Returns items</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If forum not found</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Route("/api/tcounters/{topicId}")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetTopicCounter(int topicId)
        {
            var topicCountersFromDb = await _repository.ForumTopicCounter.GetPostCounterAsync(topicId, trackChanges: false);
            var topicsDto = _mapper.Map<ForumTopicCounterDto>(topicCountersFromDb);

            return Ok(topicsDto);
        }
        [HttpPut]
        [Route("/api/tcounters/{topicId}")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [ServiceFilter(typeof(ValidateTopicCounter))]
        public async Task<IActionResult> UpdateTopicCounter(int topicId, 
            [FromBody] ForumTopicCounterForUpdateDto topicCounter)
        {
            var topicCounterEntity = HttpContext.Items["topicCounter"] as ForumTopicCounter;

            _mapper.Map(topicCounter, topicCounterEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
        [HttpPatch]
        [Route("/api/tcounters/{topicId}")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [ServiceFilter(typeof(ValidateTopicCounter))]
        public async Task<IActionResult> PatchTopicCounter(int topicId, 
            [FromBody] JsonPatchDocument<ForumTopicCounterForUpdateDto> topicCounterDoc)
        {
            if (topicCounterDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var topicCounterEntity = HttpContext.Items["topicCounter"] as ForumTopicCounter;
            var topicCounterToPatch = _mapper.Map<ForumTopicCounterForUpdateDto>(topicCounterEntity);
            topicCounterDoc.ApplyTo(topicCounterToPatch, ModelState);

            TryValidateModel(topicCounterToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(topicCounterToPatch, topicCounterEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Route("/api/tcounters")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetTopicCounters()
        {
            var topicCountersFromDb = await _repository.ForumTopicCounter.GetPostCountersAsync(trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(topicCountersFromDb.MetaData));

            var topicsDto = _mapper.Map<IEnumerable<ForumTopicCounterDto>>(topicCountersFromDb);

            return Ok(topicsDto);
        }
        /// <summary>
        /// Gets the topic
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <returns>The topic</returns>
        /// <response code="200">Returns item</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If the category or forum doesn't exist</response>
        [HttpGet("{topicId}", Name = "GetTopicForForum")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetTopicForForum(int categoryId, int forumId, int topicId, 
            [FromQuery] ForumTopicParameters forumTopicParameters)
        {
            var forumDb = await _repository.ForumBase.GetForumFromCategoryAsync(categoryId, forumId, trackChanges: false);
            if (forumDb == null)
            {
                _logger.LogInfo($"Forum with category id: {categoryId} and forum id: {forumId} doesn't exist in the database.");
                return NotFound();
            }
            var topicDb = await _repository.ForumTopic.GetTopicAsync(forumId, topicId, trackChanges: false);
            if (topicDb == null)
            {
                _logger.LogInfo($"Topic with id: {topicId} doesn't exist in the database.");
                return NotFound();
            }

            var topicDto = _mapper.Map<ForumTopicDto>(topicDb);
            var links = _topicLinks.TryGenerateLinks(new List<ForumTopicDto>() 
            { topicDto }, categoryId, forumId, forumTopicParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        /// <summary>
        /// Gets the topic collection
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="ids"></param>
        /// <returns>The topic collection</returns>
        /// <response code="200">Returns items</response>
        /// <response code="400">If the id's is empty</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If some ids are not valid in a collection</response>
        [HttpGet("collection/({ids})", Name = "TopicCollection")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetTopicCollection(int categoryId, int forumId, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids,
            [FromQuery] ForumTopicParameters forumTopicParameters)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var topicEntities = await _repository.ForumTopic.GetTopicsFromForumByIdsAsync(forumId, ids, trackChanges: false);

            if (ids.Count() != topicEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var topicsToReturn = _mapper.Map<IEnumerable<ForumTopicDto>>(topicEntities);
            var links = _topicLinks.TryGenerateLinks(topicsToReturn, categoryId, forumId, forumTopicParameters.Fields, HttpContext, ids);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        /// <summary>
        /// Creates a newly created topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns>A newly created topic</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTopicForForum(int categoryId, int forumId, [FromBody] ForumTopicForCreationDto topic)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ForumTopicForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var forum = await _repository.ForumBase.GetForumFromCategoryAsync(categoryId, forumId, trackChanges: false);
            if (forum == null)
            {
                _logger.LogInfo($"Forum with category id: {categoryId} and forum id: {forumId} doesn't exist in the database.");

                return NotFound();
            }

            var topicEntity = _mapper.Map<ForumTopic>(topic);
            topicEntity.CreatedAt = DateTime.Now;
            topicEntity.ForumBaseId = forumId;
            _repository.ForumTopic.CreateTopicForForum(forumId, topicEntity);
            await _repository.SaveAsync();

            var topicToReturn = _mapper.Map<ForumTopicDto>(topicEntity);

            return CreatedAtRoute("GetTopicForForum", new { categoryId, forumId, topicId = topicToReturn.Id }, topicToReturn);
        }
        /// <summary>
        /// Creates a newly created topic collection
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicCollection"></param>
        /// <returns>A newly created topic collection</returns>
        /// <response code="201">Returns the newly created items</response>
        /// <response code="400">If the items is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost("collection")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTopicCollectionForForum(int categoryId, int forumId, 
            [FromBody] IEnumerable<ForumTopicForCreationDto> topicCollection)
        {
            if (topicCollection == null)
            {
                _logger.LogError("Topic collection sent from client is null.");
                return BadRequest("Topic collection is null");
            }

            var topicEntities = _mapper.Map<IEnumerable<ForumTopic>>(topicCollection);

            foreach (var topic in topicEntities)
            {
                topic.ForumBaseId = forumId;
                _repository.ForumTopic.CreateTopicForForum(forumId, topic);
            }

            await _repository.SaveAsync();
            var topicCollectionToReturn = _mapper.Map<IEnumerable<ForumTopicDto>>(topicEntities);
            var ids = string.Join(",", topicCollectionToReturn.Select(c => c.Id));

            return CreatedAtRoute("TopicCollection", new { categoryId, forumId, ids }, topicCollectionToReturn);
        }
        /// <summary>
        /// Deletes existing topic
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <response code="204">If topic is deleted</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If forum or topic doesn't exist</response>
        [HttpDelete("{topicId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateTopicForForumExistsAttribute))]
        public async Task<IActionResult> DeleteTopicForForum(int categoryId, int forumId, int topicId)
        {
            var topicForForum = HttpContext.Items["topic"] as ForumTopic;

            _repository.ForumTopic.DeleteTopic(topicForForum);
            await _repository.SaveAsync();

            return NoContent();
        }
        /// <summary>
        /// Updates existing topic
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <param name="topic"></param>
        /// <response code="204">If topic is updated</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPut("{topicId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTopicForForumExistsAttribute))]
        public async Task<IActionResult> UpdateTopicForForum(int categoryId, int forumId, int topicId, [FromBody] ForumTopicForUpdateDto topic)
        {
            var topicEntity = HttpContext.Items["topic"] as ForumTopic;

            _mapper.Map(topic, topicEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
        /// <summary>
        /// Partially updates existing topic
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <param name="patchDoc"></param>
        /// <response code="204">If topic is updated</response>
        /// <response code="400">If path doc is null</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPatch("{topicId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidateTopicForForumExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateTopicForForum(int categoryId, int forumId, int topicId, 
            [FromBody] JsonPatchDocument<ForumTopicForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var topicEntity = HttpContext.Items["topic"] as ForumTopic;
            var topicToPatch = _mapper.Map<ForumTopicForUpdateDto>(topicEntity);
            patchDoc.ApplyTo(topicToPatch, ModelState);

            TryValidateModel(topicToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(topicToPatch, topicEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}