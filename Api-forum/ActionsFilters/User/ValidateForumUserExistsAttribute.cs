using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace api_forum.ActionsFilters.User
{
    public class ValidateForumUserExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        //private readonly ILoggerManager _logger;
        public ValidateForumUserExistsAttribute(IRepositoryManager repository)
        {
            _repository = repository;
            //_logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") || method.Equals("PATCH") ? true : false;
            var userId = (int)context.ActionArguments["userId"];
            var user = await _repository.ForumUsers.GetUserAsync(userId, trackChanges);

            if (user == null)
            {
                //_logger.LogInformation($"Forum user with id: {userId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("user", user);
                await next();
            }
        }
    }
}
