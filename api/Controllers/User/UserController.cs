﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using api.Business.User;
using api.Helpers;
using api.Models.Common;
using api.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.User
{
    [Route("/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusinessLogic _business;

        public UserController(IUserBusinessLogic userBusinessLogic)
        {
            _business = userBusinessLogic;
        }

        [HttpGet("self")]
        public ActionResult<GetDto<UserPrivateDto>> GetSelf()
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            return Ok(new GetDto<UserPrivateDto>(_business.GetSelf(currentUser)));
        }

        [HttpGet("{id}")]
        public ActionResult<GetDto<IUserDto>> GetUser(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            return Ok(new GetDto<IUserDto>(_business.GetUserById(id, currentUser)));
        }

        [HttpGet]
        public ActionResult<GetAllDto<UserPublicDto>> GetAll([FromQuery] PagingDto paging)
        {
            return Ok(new GetAllDto<UserPublicDto>(_business.GetUsers(paging)));
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<GetDto<UserPrivateDto>> Post([FromForm] UserCreationDto newUser)
        {
            return Created("User", new GetDto<UserPrivateDto>(_business.AddNewUser(newUser)));
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<UserPrivateDto>> Patch(string id, [FromForm] UserUpdateDto newUser)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            return Ok(new GetDto<UserPrivateDto>(_business.UpdateUserById(id, newUser, currentUser)));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            _business.DeleteUserById(id, currentUser);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<GetDto<UserLoginResponseDto>> Login([FromForm] UserLoginDto loginDto)
        {
            return Ok(new GetDto<UserLoginResponseDto>(_business.Login(loginDto)));
        }

        [HttpGet("search/{search}")]
        public ActionResult<GetAllDto<IUserDto>> SearchUser(string search, [FromQuery] PagingDto paging,
            [FromQuery] bool strict = false)
        {
            var currentUser = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            var result = _business.SearchUser(search, paging, currentUser, strict);

            return result.items.GetType().GetProperty("Item")?.PropertyType.ToString() switch
            {
                "api.Models.User.UserPublicDto" => Ok(new GetAllDto<UserPublicDto>((result.page, result.pageSize,
                    result.maxPage, (List<UserPublicDto>) result.items))),
                "api.Models.User.UserPrivateDto" => Ok(new GetAllDto<UserPrivateDto>((result.page, result.pageSize,
                    result.maxPage, (List<UserPrivateDto>) result.items))),
                _ => Ok(new GetAllDto<UserPublicDto>((result.page, result.pageSize, result.maxPage,
                    (List<UserPublicDto>) result.items)))
            };
        }
    }
}