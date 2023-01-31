using HotelList.API.IRepository;
using HotelList.API.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
        {
            _authManager = authManager;
            _logger = logger;
        }

        // POST: api/aacount/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//this will update swagger so that it knows, it might return a bad request.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//this will update swagger so that it knows, it might return server internal error.
        [ProducesResponseType(StatusCodes.Status200OK)]//this will update swagger so that it knows, it might return Ok response.
        public async Task<ActionResult> Register([FromBody]UserDto userDto)
        {
            //logger
            _logger.LogInformation($"Registration Attempt for {userDto.Email}");

            var errors = await _authManager.Register(userDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok();
        }

        // POST: api/account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//this will update swagger so that it knows, it might return a bad request.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//this will update swagger so that it knows, it might return server internal error.
        [ProducesResponseType(StatusCodes.Status200OK)]//this will update swagger so that it knows, it might return Ok response.
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            //logger
            _logger.LogInformation($"Login Attempt for {loginDto.Email}");

            var authResponse = await _authManager.Login(loginDto);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            return Ok(authResponse); 
        }

        // POST: api/account/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//this will update swagger so that it knows, it might return a bad request.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//this will update swagger so that it knows, it might return server internal error.
        [ProducesResponseType(StatusCodes.Status200OK)]//this will update swagger so that it knows, it might return Ok response.
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            var authResponse = await _authManager.VerifyRefreshToken(request);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }

    }
}
