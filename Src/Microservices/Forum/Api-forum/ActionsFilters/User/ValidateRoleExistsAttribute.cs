using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Entities.DTO.UserDto.Create;
using Entities.Models;

namespace api_forum.ActionsFilters.User
{
    public class ValidateRoleExistsAttribute : IAsyncActionFilter
    {
        private readonly ILogger<ValidateRoleExistsAttribute> _logger;
        private readonly RoleManager<AppRole> _roleManager;
        public ValidateRoleExistsAttribute(ILogger<ValidateRoleExistsAttribute> logger, RoleManager<AppRole> roleManager)
        {
            _logger = logger;
            _roleManager = roleManager;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            var userDto = (UserForCreationDto)context.ActionArguments.SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;

            if (userDto.Roles.Count > 0)
            {
                foreach (var role in userDto.Roles)
                {
                    var isRoleExist = await _roleManager.RoleExistsAsync(role);

                    if (!isRoleExist)
                    {
                        _logger.LogError($"User role {role} does not exist in database");
                        context.Result = new BadRequestObjectResult($"User role {role} does not exist in database");
                        return;
                    }
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult($"Please provide roles for user.");
                return;
            }

            await next();
        }
    }
}