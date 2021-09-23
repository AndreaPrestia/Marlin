using System.Collections.Generic;

namespace Marlin.Core.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public long Mobile { get; set; }
        public long Created { get; set; }
        public long Disabled { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public List<Role> Roles { get; set; }
    }
}
