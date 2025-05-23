using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Workshop.Shared.Services;
using Workshop.WebApi.Authentication.Infrastructure;
using Workshop.WebApi.Authentication.Infrastructure.Authentication;
using Workshop.WebApi.Authentication.Infrastructure.Exceptions;
using Workshop.WebApi.Authentication.Models;

namespace Workshop.WebApi.Authentication.Controllers
{
    /// <summary>
    /// A set of APIs used for user authentication.
    /// </summary>
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
        /// <param name="request">A payload with user credentials.</param>
        /// <returns>An operation result.</returns>
        /// <response code="200">Login was successful.</response>
        /// <response code="401">Invalid credentials.</response>
        [AllowAnonymous]
        [HttpPost]
        [Consumes(typeof(CredentialsModel), Constants.ContentType.Json)]
        public async Task<IActionResult> CookieLogin([FromBody]CredentialsModel request)
        {
            if (!await ValidateLogin(request))
            {
                return Unauthorized();
            }
    
            CreateAuthenticationCookie(request);
            return Ok();
        }

        /// <summary>
        /// Accepts credentials from users and returns pairs of tokens to the authenticated ones.
        /// </summary>
        /// <param name="request">A payload with user credentials.</param>
        /// <returns>An operation result.</returns>
        /// <response code="200">Login was successful.</response>
        /// <response code="400">Invalid payload.</response>
        [AllowAnonymous]
        [HttpPost("jwt")]
        [Consumes(typeof(CredentialsModel), Constants.ContentType.Json)]
        public async Task<IActionResult> JwtLogin([FromBody]CredentialsModel request)
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

        /// <summary>
        /// Use a refresh token to generate a valid access token.
        /// </summary>
        /// <param name="request">A payload with a valid refresh token.</param>
        /// <returns>An operation result.</returns>
        /// <response code="200">Tokens were provided.</response>
        /// <response code="400">Invalid payload.</response>
        /// <response code="401">Authentication failure.</response>
        [Authorize(AuthenticationSchemes = Constants.Authentication.JwtSchemaName)]
        [HttpPost("refresh")]
        [Consumes(typeof(RefreshTokenModel), Constants.ContentType.Json)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel request)
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
