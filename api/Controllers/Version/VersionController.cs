﻿using api.Business.Version;
using api.Helpers;
using api.Models.Common;
using api.Models.Version;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers.Version
{
    [Route("/v1/versions")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly IVersionBusinessLogic _business;
        
        public VersionController(IVersionBusinessLogic versionBusinessLogic)
        {
            _business = versionBusinessLogic;
        }

        [HttpGet("{id}")]
        public ActionResult<GetDto<VersionPublicDto>> GetVersion(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);
            
            return Ok(new GetDto<VersionPublicDto>(_business.GetVersionById(id, currentUser)));
        }

        [HttpGet]
        public ActionResult<GetAllDto<VersionPublicDto>> GetAll([FromQuery] PagingDto paging, [FromQuery] VersionFiltersDto filters)
        {
            var currentUser = new ConnectedUser(User.Claims);
            
            return Ok(new GetAllDto<VersionPublicDto>(_business.GetVersions(paging, filters, currentUser)));
        }

        [HttpPost]
        public ActionResult<GetDto<VersionPublicDto>> Post([FromForm] VersionCreationDto newVersion)
        {
            var currentUser = new ConnectedUser(User.Claims);
            
            return Created("Version", new GetDto<VersionPublicDto>(_business.AddNewVersion(newVersion, currentUser)));
        }

        [HttpPatch("{id}")]
        public ActionResult<GetDto<VersionPublicDto>> Patch(string id, [FromForm] VersionUpdateDto newVersion)
        {
            var currentUser = new ConnectedUser(User.Claims);
            
            return Ok(new GetDto<VersionPublicDto>(_business.UpdateVersionById(id, newVersion, currentUser)));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var currentUser = new ConnectedUser(User.Claims);
            
            _business.DeleteVersionById(id, currentUser);
            
            return Ok();
        }
    }
}