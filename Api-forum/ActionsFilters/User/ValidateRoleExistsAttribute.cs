using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Entities.DTO.UserDto;
using Entities.DTO.UserDto.Create;
using Entities.Models;

namespace api_forum.ActionsFilters.User
{
    public class ValidateRoleExistsAttribute : IAsyncActionFilter
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ILoggerManager _logger;
        public ValidateRoleExistsAttribute(RoleManager<AppRole> roleManager, ILoggerManager logger)
        {
            _roleManager = roleManager;
            _logger = logger;
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