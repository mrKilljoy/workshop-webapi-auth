using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Workshop.Shared.Services;
using Workshop.WebApi.Cookie.Infrastructure;
using Workshop.WebApi.Cookie.Infrastructure.Authentication;
using Workshop.WebApi.Cookie.Models;

namespace Workshop.WebApi.Cookie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IDataProtector _dataProtector;

        public LoginController(IDataProtectionProvider idp, IUserManager userManager)
        {
            _userManager = userManager;
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
    }
}
