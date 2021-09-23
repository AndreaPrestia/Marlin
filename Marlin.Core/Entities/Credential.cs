namespace Marlin.Core.Entities
{
    public class Credential : BaseEntity
    {
        public string Value { get; set; }
        public long Created { get; set; }
        public long Deleted { get; set; }
    }
}
