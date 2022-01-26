﻿using System.Threading.Tasks;
using EShop.Api.Helpers;
using EShop.Api.Models.Profile;
using EShop.Domain.Identity;
using EShop.Domain.Profile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace EShop.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly ICurrentUserProvider _currentUserProvider;

        public ProfileController(IProfileService profileService, ICurrentUserProvider currentUserProvider)
        {
            _profileService = profileService;
            _currentUserProvider = currentUserProvider;
        }
        
        /// <summary>
        /// Get profile
        /// </summary>
        /// <response code="200">Profile successfully returned</response>
        /// <response code="404">Profile not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfile>> GetProfileByIdAsync()
        {
            var profile = await _profileService.FindProfileByIdAsync(_currentUserProvider.UserId);

            if (profile == null)
            {
                return NotFound();
            }
            
            return Ok(profile);
        }

        /// <summary>
        /// Update profile
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="model">update profile model</param>
        /// <response code="204">Profile successfully updated</response>
        /// <response code="404">Nice try</response>
        /// <response code="400">Problem results</response>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserProfile>> UpdateProfileAsync([FromRoute] string userId, [FromBody] UpdateProfileViewModel model)
        {
            if (userId != _currentUserProvider.UserId)
            {
                return BadRequest();
            }
            
            var profile = await _profileService.FindProfileByIdAsync(_currentUserProvider.UserId);

            if (profile == null)
            {
                return NotFound();
            }

            var result = await _profileService.UpdateProfileAsync(_currentUserProvider.UserId, model.FirstName,
                model.LastName, model.PhoneNumber);

            if (result.Successed)
            {
                return NoContent();
            }

            return BadRequest(result.ToProblemDetails());
        }
    }
}