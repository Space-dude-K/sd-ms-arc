using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace api_forum.ActionsFilters.Forum
{
    public class ValidatePostForTopicExistsAttribute : IAsyncActionFilter
    {
        private readonly ILogger<ValidatePostForTopicExistsAttribute> _logger;
        private readonly IRepositoryManager _repository;
        public ValidatePostForTopicExistsAttribute(ILogger<ValidatePostForTopicExistsAttribute> logger, 
            IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") || method.Equals("PATCH") ? true : false;
            var forumId = (int)context.ActionArguments["forumId"];
            var topicId = (int)context.ActionArguments["topicId"];
            var topic = await _repository.ForumTopic.GetTopicAsync(forumId, topicId, false);

            if (topic == null)
            {
                _logger.LogInformation($"Topic with forum id: {forumId} and topic id: {topicId} doesn't exist in the database.");
                context.Result = new NotFoundResult();

                return;
            }

            var postId = (int)context.ActionArguments["postId"];
            var post = await _repository.ForumPost.GetPostAsync(topicId, postId, trackChanges);

            if (post == null)
            {
                _logger.LogInformation($"Post with id: {postId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("post", post);
                await next();
            }
        }
    }
}