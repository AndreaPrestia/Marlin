using System.Collections.Generic;

namespace Marlin.Core.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public List<Resource> Resources { get; set; }
    }
}
