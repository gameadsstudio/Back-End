using System;
using System.Collections.Generic;
using GAME_ADS_STUDIO_API.Data.User;
using GAME_ADS_STUDIO_API.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace GAME_ADS_STUDIO_API.Controllers
{
    [Route("/api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepo = new UserRepository();

        // GET api/user
        [HttpGet]
        public ActionResult<IEnumerable<UserModel>> getAllUsers()
        {
            var allUsers = _userRepo.GetUsers();

            return Ok(allUsers);
        }

        //GET api/user/{id}
        [HttpGet("{id}")]
        public ActionResult <UserModel> getUserById(int id)
        {
            var user = _userRepo.GetUserById(id);

            return Ok(user);
        }

    }
}
