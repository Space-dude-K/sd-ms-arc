using AutoMapper;
using Interfaces;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.Models.Forum;
using Entities.RequestFeatures.Forum;
using Forum.ModelBinders;
using Forum.Utility.ForumLinks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using api_forum.ActionsFilters.Forum;
using api_forum.ActionsFilters;

namespace Forum.Controllers.Forum
{
    [Route("api/categories")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, User")]
    public class CategoryController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly CategoryLinks _categoryLinks;

        // TODO. Service layer for mappings and data shaping
        public CategoryController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, CategoryLinks categoryLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _categoryLinks = categoryLinks;
        }
        /// <summary>
        /// Gets allowed categories options
        /// </summary>
        /// <returns>The options list</returns>
        /// <response code="200">Returns items</response>
        /// <response code="401">If unauthorized</response>
        [HttpOptions]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult GetCategoriesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
        /// <summary>
        /// Gets the list of all categories
        /// </summary>
        /// <returns>The categories list</returns>
        /// <response code="200">Returns items</response>
        /// <response code="401">If unauthorized</response>
        [HttpGet(Name = "GetCategories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetCategories([FromQuery] ForumCategoryParameters forumCategoryParameters)
        {
            var categoriesFromDb = await _repository.ForumCategory.GetAllCategoriesAsync(forumCategoryParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(categoriesFromDb.MetaData));

            var categoriesDto = _mapper.Map<IEnumerable<ForumCategoryDto>>(categoriesFromDb);
            var links = _categoryLinks.TryGenerateLinks(categoriesDto, forumCategoryParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        /// <summary>
        /// Gets the category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>The category</returns>
        /// <response code="200">Returns item</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If the category doesn't exist</response>
        [HttpGet("{categoryId}", Name = "GetCategoryById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetCategory(int categoryId, 
            [FromQuery] ForumCategoryParameters forumCategoryParameters)
        {
            var category = await _repository.ForumCategory.GetCategoryAsync(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Category with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var categoryDto = _mapper.Map<ForumCategoryDto>(category);
                var links = _categoryLinks.TryGenerateLinks(new List<ForumCategoryDto>() { categoryDto }, forumCategoryParameters.Fields, HttpContext);

                return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
            }
        }
        /// <summary>
        /// Gets the category collection
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>The category collection</returns>
        /// <response code="200">Returns items</response>
        /// <response code="400">If the id's is empty</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If some ids are not valid in a collection</response>
        [HttpGet("collection/({ids})", Name = "CategoryCollection")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetCategoryCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids, 
            [FromQuery] ForumCategoryParameters forumCategoryParameters)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var categoryEntities = await _repository.ForumCategory.GetCategoriesByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != categoryEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var categoriesToReturn = _mapper.Map<IEnumerable<ForumCategoryDto>>(categoryEntities);
            var links = _categoryLinks.TryGenerateLinks(categoriesToReturn, forumCategoryParameters.Fields, HttpContext, ids);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        /// <summary>
        /// Creates a newly created category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>A newly created category</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost(Name = "CreateCategory")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCategory([FromBody] ForumCategoryForCreationDto category)
        {
            var categoryEntity = _mapper.Map<ForumCategory>(category);

            if (categoryEntity == null)
            {
                _logger.LogError("ForumCategory entity is null");
                return BadRequest("ForumCategory entity is null");
            }

            _repository.ForumCategory.CreateCategory(categoryEntity);
            await _repository.SaveAsync();

            var categoryToReturn = _mapper.Map<ForumCategoryDto>(categoryEntity);

            return CreatedAtRoute("GetCategoryById", new { categoryId = categoryToReturn.Id }, categoryToReturn);
        }
        /// <summary>
        /// Creates a newly created category collection
        /// </summary>
        /// <param name="categoryCollection"></param>
        /// <returns>A newly created category collection</returns>
        /// <response code="201">Returns the newly created items</response>
        /// <response code="400">If the items is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost("collection")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCategoryCollection([FromBody] IEnumerable<ForumCategoryForCreationDto> categoryCollection)
        {
            if (categoryCollection == null)
            {
                _logger.LogError("Category collection sent from client is null.");
                return BadRequest("Category collection is null");
            }
            var categoryEntities = _mapper.Map<IEnumerable<ForumCategory>>(categoryCollection);
            foreach (var category in categoryEntities)
            {
                _repository.ForumCategory.CreateCategory(category);
            }
            await _repository.SaveAsync();
            var categoryCollectionToReturn = _mapper.Map<IEnumerable<ForumCategoryDto>>(categoryEntities);
            var ids = string.Join(",", categoryCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("CategoryCollection", new { ids }, categoryCollectionToReturn);
        }
        /// <summary>
        /// Updates existing category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="category"></param>
        /// <response code="204">If category is updated</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPut("{categoryId}", Name = "UpdateCategory")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] ForumCategoryForUpdateDto category)
        {
            var categoryEntity = HttpContext.Items["category"] as ForumCategory;

            if (categoryEntity == null)
            {
                _logger.LogError("ForumCategory entity is null");
                return BadRequest("ForumCategory entity is null");
            }

            _mapper.Map(category, categoryEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        /// <summary>
        /// Partially updates existing category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="patchDoc"></param>
        /// <response code="204">If category is updated</response>
        /// <response code="400">If path doc is null</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPatch("{categoryId}", Name = "PartiallyUpdateCategory")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateCategory(int categoryId, 
            [FromBody] JsonPatchDocument<ForumCategoryForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var categoryEntity = HttpContext.Items["category"] as ForumCategory;

            var categoryToPatch = _mapper.Map<ForumCategoryForUpdateDto>(categoryEntity);
            patchDoc.ApplyTo(categoryToPatch, ModelState);

            TryValidateModel(categoryToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(categoryToPatch, categoryEntity);

            _logger.LogInfo($"Part up {categoryToPatch.Name}");

            await _repository.SaveAsync();

            return NoContent();
        }
        /// <summary>
        /// Deletes existing category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <response code="204">If category is deleted</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If category doesn't exist</response>
        [HttpDelete("{categoryId}", Name = "DeleteCategory")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = HttpContext.Items["category"] as ForumCategory;

            if (category == null)
            {
                _logger.LogError("ForumCategory entity is null");
                return BadRequest("ForumCategory entity is null");
            }

            _repository.ForumCategory.DeleteCategory(category);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}