using System;

namespace Marlin.Core.Models
{
    public class ResourceUpdateRequest
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public bool IsPublic { get; set; }
        public Guid ParentId { get; set; }
        public int Order { get; set; }
    }
}
