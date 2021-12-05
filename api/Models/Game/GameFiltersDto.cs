using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Models.Game
{
    public class GameFiltersDto
    {
        [BindRequired]
        public Guid OrganizationId { get; set; }
    }
}