using Api_auth.Models;
using Api_auth_Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api_auth.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthManager _authenticationManager;

        public AuthController(ILogger<AuthController> logger,      
            IAuthManager authenticationManager)
        {
            _logger = logger;
            _authenticationManager = authenticationManager;
        }
        [HttpPost("login")]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authenticationManager.ValidateUser(user))
            {
                _logger.LogError($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
                return Unauthorized();
            }

            return Ok(new { Token = await _authenticationManager.CreateToken() });
        }
    }
}