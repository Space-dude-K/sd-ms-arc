using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace api_forum.ActionsFilters.Forum
{
    public class ValidateTopicForForumExistsAttribute : IAsyncActionFilter
    {
        private readonly ILogger<ValidateTopicForForumExistsAttribute> _logger;
        private readonly IRepositoryManager _repository;
        public ValidateTopicForForumExistsAttribute(ILogger<ValidateTopicForForumExistsAttribute> logger, 
            IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") || method.Equals("PATCH") ? true : false;
            var categoryId = (int)context.ActionArguments["categoryId"];
            var forumId = (int)context.ActionArguments["forumId"];
            var forum = await _repository.ForumBase.GetForumFromCategoryAsync(categoryId, forumId, false);

            if (forum == null)
            {
                _logger.LogInformation($"Forum with category id: {categoryId} and forum id: {forumId} doesn't exist in the database.");
                context.Result = new NotFoundResult();

                return;
            }

            var topicId = (int)context.ActionArguments["topicId"];
            var topic = await _repository.ForumTopic.GetTopicAsync(forumId, topicId, trackChanges);

            if (topic == null)
            {
                _logger.LogInformation($"Topic with id: {topicId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("topic", topic);
                await next();
            }
        }
    }
}