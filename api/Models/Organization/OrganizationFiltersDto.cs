using System;

namespace api.Models.Organization
{
    public class OrganizationFiltersDto
    {
        public Guid? UserId { get; set; }

        public string Name { get; set; }
    }
}