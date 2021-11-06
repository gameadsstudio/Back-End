using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.AdContainer
{
    public class AdContainerFiltersDto
    {
        [BindRequired]
        public Guid OrganizationId { get; set; }
        
        public Guid VersionId { get; set; }
    }
}