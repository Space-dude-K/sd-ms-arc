using Entities.DTO.UserDto.Create;
using Entities.DTO.UserDto;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Auth_Interfaces;

namespace Api_authentication.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        //private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAuthenticationManager _authenticationManager;
        //private readonly IRepositoryManager _repository;

        public AuthenticationController(ILogger<AuthenticationController> logger, 
            UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
            IAuthenticationManager authenticationManager)
        {
            _logger = logger;
            //_mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationManager = authenticationManager;
            //_repository = repository;
        }
        //[HttpPost()]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        //[ServiceFilter(typeof(ValidateRoleExistsAttribute))]
        /*public async Task<IActionResult> RegisterUser([FromBody] UserForCreationDto userForRegistration)
        {
            var user = _mapper.Map<AppUser>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            _repository.ForumUsers.CreateForumUser(user.Id);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);

            return StatusCode(201);
        }*/
        [HttpPost("login")]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authenticationManager.ValidateUser(user))
            {
                //_logger.LogError($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
                return Unauthorized();
            }

            return Ok(new { Token = await _authenticationManager.CreateToken() });
        }
    }
}
