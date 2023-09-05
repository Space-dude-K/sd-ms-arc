using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace api_forum.ActionsFilters.Forum
{
    public class ValidatePostForTopicExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        //private readonly ILoggerManager _logger;
        public ValidatePostForTopicExistsAttribute(IRepositoryManager repository)
        {
            _repository = repository;
            //_logger = logger;
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
                //_logger.LogInfo($"Topic with forum id: {forumId} and topic id: {topicId} doesn't exist in the database.");
                context.Result = new NotFoundResult();

                return;
            }

            var postId = (int)context.ActionArguments["postId"];
            var post = await _repository.ForumPost.GetPostAsync(topicId, postId, trackChanges);

            if (post == null)
            {
                //_logger.LogInfo($"Post with id: {postId} doesn't exist in the database.");
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