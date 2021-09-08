using System.ComponentModel.DataAnnotations;

namespace Marlin.Core.Models
{
    public class RoleAddRequest
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
