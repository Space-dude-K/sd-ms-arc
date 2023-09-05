using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace api_forum.ActionsFilters.Forum
{
    public class ValidateTopicCounter : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        //private readonly ILoggerManager _logger;
        public ValidateTopicCounter(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            //_logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") || method.Equals("PATCH") ? true : false;
            var topicId = (int)context.ActionArguments["topicId"];
            var topicCounter = await _repository.ForumTopicCounter.GetPostCounterAsync(topicId, trackChanges);

            if (topicCounter == null)
            {
                ////_logger.LogInfo($"Topic counter with id: {topicId} doesn't exist in the database.");
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
