using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MovieLibrary.Data.Models;
using MovieLibrary.Services.Data.JwtService;
using MovieLibrary.Services.Data.UsersService;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieLibrary.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService userService;
        private readonly ILogger<UsersController> logger;
        private readonly MovieLibrary.Services.Data.JwtService.SingInManager signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtAuthService jwtAuthService;

        public UsersController(
            IUsersService userService,
            ILogger<UsersController> logger,
           MovieLibrary.Services.Data.JwtService.SingInManager signInManager,
            UserManager<ApplicationUser> userManager,
            JwtAuthService jwtAuthService)
        {
            this.userService = userService;
            this.logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.jwtAuthService = jwtAuthService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] CreateUserModel model)
        {
            try
            {
                await userService.CreateAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var result = await this.signInManager.SignIn(request.UserName, request.Password);

            if (!result.Success) return Unauthorized();

            this.logger.LogInformation($"User [{request.UserName}] logged in the system.");

            return Ok(new LoginResult
            {
                User = result.User,
                UserId = result.User.Id,
                UserName = result.User.UserName,
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken
            });
        }

        [HttpPost("user")]
        public string UserAuth([FromBody] string accessToken)
        {

            ClaimsPrincipal claimsPrincipal = this.jwtAuthService.GetPrincipalFromToken(accessToken);
            string id = claimsPrincipal.Claims.First(c => c.Type == "id").Value;

            var userId = JsonConvert.SerializeObject(id);
            return userId;

        }

        [HttpPost("refreshtoken")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var result = await this.signInManager.RefreshToken(request.AccessToken, request.RefreshToken);

            if (!result.Success) return Unauthorized();

            return Ok(new LoginResult
            {
                User = result.User,
                UserId = result.User.Id,
                UserName = result.User.Email,
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
            });
        }

        [AllowAnonymous]
        [HttpGet("getUser/id")]
        public string GetUserById(string userId)
        {
            var user = this.userService.GetById(userId);

            var json = JsonConvert.SerializeObject(user);

            return json;
        }
    }
}

