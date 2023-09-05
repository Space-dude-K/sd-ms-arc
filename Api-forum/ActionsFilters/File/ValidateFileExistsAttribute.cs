using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace api_forum.ActionsFilters.File
{
    public class ValidateFileExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;

        public ValidateFileExistsAttribute(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = method.Equals("PUT") || method.Equals("PATCH") ? true : false;
            var forumUserId = (int)context.ActionArguments["forumUserId"];
            var file = await _repository.ForumFile.GetFileAsync(forumUserId, trackChanges);

            if (file == null)
            {
                //_logger.LogInfo($"File with user id: {forumUserId} doesn't exist in the database.");
                context.Result = new NotFoundResult();

                return;
            }
            else
            {
                context.HttpContext.Items.Add("file", file);
                await next();
            }
        }
    }
}
