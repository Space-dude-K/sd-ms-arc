using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace api_forum.ActionsFilters.Forum
{
    public class ValidateForumForCategoryExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateForumForCategoryExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") || method.Equals("PATCH") ? true : false;
            var categoryId = (int)context.ActionArguments["categoryId"];
            var category = await _repository.ForumCategory.GetCategoryAsync(categoryId, false);
            if (category == null)
            {
                _logger.LogInfo($"Category with id: {categoryId} doesn't exist in the database.");

                context.Result = new NotFoundResult();
                return;
            }
            var forumId = (int)context.ActionArguments["forumId"];
            var forum = await _repository.ForumBase.GetForumFromCategoryAsync(categoryId, forumId, trackChanges);
            if (forum == null)
            {
                _logger.LogInfo($"Forum with id: {forumId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("forum", forum);
                await next();
            }
        }
    }
}
