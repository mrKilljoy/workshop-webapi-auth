using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Workshop.Shared.Services;
using Workshop.WebApi.Cookie.Infrastructure;
using Workshop.WebApi.Cookie.Infrastructure.Authentication;
using Workshop.WebApi.Cookie.Infrastructure.Exceptions;
using Workshop.WebApi.Cookie.Models;

namespace Workshop.WebApi.Cookie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ITokenManager _tokenManager;
        private readonly IDataProtector _dataProtector;

        public LoginController(IDataProtectionProvider idp,
            IUserManager userManager,
            ITokenGenerator tokenGenerator,
            ITokenManager tokenManager)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _tokenManager = tokenManager;
            _dataProtector = idp.CreateProtector(Constants.Authentication.DataProtectorName);
        }
        
        /// <summary>
        /// Accepts credentials from users and returns cookies to the authenticated ones.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]CredentialsModel request)
        {
            if (!await ValidateLogin(request))
            {
                return Unauthorized();
            }
    
            CreateAuthenticationCookie(request);
            return Ok("Login was successful");
        }

        [HttpPost("jwt")]
        public async Task<IActionResult> JwtLogin([FromBody] CredentialsModel request)
        {
            if (!await ValidateLogin(request))
            {
                return Unauthorized();
            }

            var result = new TokenPairModel()
            {
                AccessToken = CreateAccessToken(request.Login),
                RefreshToken = await CreateRefreshToken(request.Login)
            };

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = Constants.Authentication.JwtSchemaName)]
        [HttpPost("jwt/refresh")]
        public async Task<IActionResult> JwtRefresh([FromBody] RefreshTokenModel request)
        {
            if (!await ValidateRefreshToken(request.RefreshToken))
                return BadRequest("Invalid token");

            var result = new TokenPairModel()
            {
                AccessToken = CreateAccessToken(GetUserLogin()),
                RefreshToken = await CreateRefreshToken(GetUserLogin())
            };
            
            return Ok(result);
        }

        private async Task<bool> ValidateLogin(CredentialsModel model)
        {
            if (model is null || string.IsNullOrEmpty(model.Login) || string.IsNullOrEmpty(model.Password))
                return false;

            return await _userManager.Get(model.Login, model.Password) is not null;
        }
        
        private void CreateAuthenticationCookie(CredentialsModel model)
        {
            var encryptedCookie = _dataProtector
                .Protect(model.Login);
            
            Response.Cookies.Append(Constants.Authentication.CookieSchemaName, encryptedCookie);
        }

        private string CreateAccessToken(string login) => _tokenGenerator.Generate(login);

        private async Task<string> CreateRefreshToken(string login)
        {
            var user  = await _userManager.Get(login);
            if (user is null)
                throw new UserNotFoundException(login);

            var item = await _tokenManager.GetToken(user.Id);
            if (item is null)
            {
                var token = await _tokenManager.CreateToken(user.Id);
                return token;
            }

            if (!item.IsValid())
            {
                await _tokenManager.RemoveToken(user.Id);
                var token = await _tokenManager.CreateToken(user.Id);
                return token;
            }

            return item.Value;
        }

        private async Task<bool> ValidateRefreshToken(string token) => await _tokenManager.IsValid(token);

        private string? GetUserLogin() => User.Identity?.Name;
    }
}
