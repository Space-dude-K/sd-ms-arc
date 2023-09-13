using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace api_forum.ActionsFilters.User
{
    public class ValidateForumUserExistsAttribute : IAsyncActionFilter
    {
        private readonly ILogger<ValidateForumUserExistsAttribute> _logger;
        private readonly IRepositoryManager _repository;
        public ValidateForumUserExistsAttribute(ILogger<ValidateForumUserExistsAttribute> logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") || method.Equals("PATCH") ? true : false;
            var userId = (int)context.ActionArguments["userId"];
            var user = await _repository.ForumUsers.GetUserAsync(userId, trackChanges);

            if (user == null)
            {
                _logger.LogError($"Forum user with id: {userId} doesn't exist in the database.");
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