using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Version
{
    public class VersionFiltersDto
    {
        public string Name { get; set; }
        
        [RegularExpression("^(0|[1-9]\\d*)\\.(0|[1-9]\\d*)\\.(0|[1-9]\\d*)(?:-((?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\\.(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\\+([0-9a-zA-Z-]+(?:\\.[0-9a-zA-Z-]+)*))?$", ErrorMessage = "The given SemVer does not comply with the semver.org specifications.")]
        public string SemVer { get; set; }
        
        [BindRequired]
        public Guid GameId { get; set; }
    }
}