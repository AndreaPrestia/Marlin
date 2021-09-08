using System;

namespace Marlin.Core.Models
{
    public class RoleAssemblyRequest
    {
        public Guid RoleId { get; set; }
        public Guid AssemblyId { get; set; }
    }
}
