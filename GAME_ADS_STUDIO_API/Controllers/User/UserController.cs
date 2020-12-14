﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAME_ADS_STUDIO_API.Business.User;
using GAME_ADS_STUDIO_API.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using GAME_ADS_STUDIO_API.Configuration;
using Microsoft.AspNetCore.Authorization;
using GAME_ADS_STUDIO_API.Models.User;

namespace GAME_ADS_STUDIO_API.Controllers
{
    [Route("/api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusinessLogic _business;

        public UserController(IOptions<AppSettings> appSettings)
        {
            _business = new UserBusinessLogic(appSettings);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var UserGet = _business.GetUserById(id);

            if (UserGet != null)
                return Ok(UserGet);
            if (id < 0)
                return BadRequest();
            return NotFound("User not found.");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromForm] UserCreationModel newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.AddNewUser(newUser);

            if (success != null)
                return Created("User", success);
            return Conflict(new { message = "Couldn't create User" });
        }

        [AllowAnonymous]
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromForm] UserUpdateModel newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.UpdateUserById(id, newUser);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => NotFound(),
                _ => BadRequest()
            };
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] UserUpdateModel newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var success = _business.UpdateUserById(id, newUser);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => NotFound(),
                _ => BadRequest()
            };
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _business.DeleteUserById(id);

            return success switch
            {
                1 => (IActionResult)Ok(),
                2 => Unauthorized(),
                _ => BadRequest()
            };
        }

    }
}
