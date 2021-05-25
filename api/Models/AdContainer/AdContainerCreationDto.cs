using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using api.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using api.Enums.Media;
using Type = api.Enums.Media.Type;

namespace api.Models.AdContainer
{
    public class AdContainerCreationDto
    {
        [Required]
        public string Name { get; set; }

        [BindRequired]
        public Guid VersionId { get; set; }

        [Required]
        public IList<string> TagNames { get; set; }

        [RequiredEnumAttribute]
        public Type Type { get; set; }

        public AspectRatio? AspectRatio { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int? Depth { get; set; }
    }
}