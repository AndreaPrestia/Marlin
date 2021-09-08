using System;
using System.Collections.Generic;

namespace Marlin.Core.Entities
{
    public class Resource : BaseEntity
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public bool IsPublic { get; set; }
        public Guid ParentId { get; set; }
        public int Order { get; set; }
        public List<Resource> Children { get; set; }
    }
}
