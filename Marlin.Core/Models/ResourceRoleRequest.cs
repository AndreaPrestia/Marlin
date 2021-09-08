using System;

namespace Marlin.Core.Models
{
    public class ResourceRoleRequest
    {
        public Guid RoleId { get; set; }
        public Guid ResourceId { get; set; }
    }
}
