using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace api_forum.ActionsFilters.Forum
{
    public class ValidateCategoryExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        //private readonly ILoggerManager _logger;
        public ValidateCategoryExistsAttribute(IRepositoryManager repository)
        {
            _repository = repository;
            //_logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT") || context.HttpContext.Request.Method.Equals("PATCH");
            var id = (int)context.ActionArguments["categoryId"];
            var category = await _repository.ForumCategory.GetCategoryAsync(id, trackChanges);
            if (category == null)
            {
                //_logger.LogInformation($"Category with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("category", category);
                await next();
            }
        }
    }
}