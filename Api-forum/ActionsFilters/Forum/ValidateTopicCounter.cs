using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace api_forum.ActionsFilters.Forum
{
    public class ValidateTopicCounter : IAsyncActionFilter
    {
        private readonly ILogger<ValidateTopicCounter> _logger;
        private readonly IRepositoryManager _repository;

        public ValidateTopicCounter(ILogger<ValidateTopicCounter> logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") || method.Equals("PATCH") ? true : false;
            var topicId = (int)context.ActionArguments["topicId"];
            var topicCounter = await _repository.ForumTopicCounter.GetPostCounterAsync(topicId, trackChanges);

            if (topicCounter == null)
            {
                _logger.LogInformation($"Topic counter with id: {topicId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("topicCounter", topicCounter);
                await next();
            }
        }
    }
}
