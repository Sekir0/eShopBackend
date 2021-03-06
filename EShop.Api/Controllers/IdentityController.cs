using System.Threading.Tasks;
using EShop.Api.Helpers;
using EShop.Api.Models.Identity;
using EShop.Domain.Identity;
using EShop.Domain.Identity.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="model">Registration model</param>
        /// <response code="200">User account successfully created</response>
        /// <response code="400">Validation failed</response>
        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RegistrationAsync([FromBody] RegistrationViewModel model)
        {
            var result = await _identityService.RegistrationAsync(model.FirstName, model.LastName, model.PhoneNumber,
                model.Email, model.ReceiveMails, model.Password, model.ConfirmPassword);

            if (!result.Successed)
            {
                return BadRequest(result.ToProblemDetails());
            }

            return Ok();
        }
        
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model">Login model</param>
        /// <response code="200">Successful login</response>
        /// <response code="400">Wrong email/password combination</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResult>> LoginAsync([FromBody] LoginViewModel model)
        {
            var (result, loginResult) = await _identityService.LoginAsync(model.Email, model.Password);

            if (result.Successed)
            {
                return Ok(loginResult);
            }
            
            return BadRequest(result.ToProblemDetails());
        }

        /// <summary>
        /// Refresh jwt token
        /// </summary>
        /// <param name="model">refresh token</param>
        /// <response code="200">Successful refreshed</response>
        /// <response code="400">Smt went wrong</response>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResult>> RefreshTokenAsync([FromBody] RefreshViewModel model)
        {
            var (result, loginResult) = await _identityService.RefreshTokenAsync(model.RefreshToken);

            if (!result.Successed)
            {
                return BadRequest(result.ToProblemDetails());
            }

            return Ok(loginResult);
        }

        /// <summary>
        /// Login by facebook access token
        /// </summary>
        /// <param name="model">Access token</param>
        /// <response code="200">Successful login</response>
        /// <response code="400">Smt went wrong</response>
        //TODO need to test this endpoint
        [HttpPost("facebook-login")]
        public async Task<ActionResult<LoginResult>> FacebookLoginAsync([FromBody] FacebookLoginViewModel model)
        {
            var (domainResult, loginResult) = await _identityService.FacebookLogin(model.AccessToken);

            if (!domainResult.Successed)
            {
                return BadRequest(domainResult.ToProblemDetails());
            }

            return Ok(loginResult);
        }

        /// <summary>
        /// Login by google access token
        /// </summary>
        /// <param name="model">access token</param>
        /// <response code="200">Successful login</response>
        /// <response code="400">Smt went wrong</response>
        [HttpPost("google-login")]
        public async Task<ActionResult<LoginResult>> GoogleLoginAsync([FromBody] GoogleLoginViewModel model)
        {
            var (domainResult, loginResult) = await _identityService.GoogleLogin(model.AccessToken);

            if (!domainResult.Successed)
            {
                return BadRequest(domainResult.ToProblemDetails());
            }

            return Ok(loginResult);
        }
    }
}