using System;
using System.ComponentModel.DataAnnotations;

namespace Marlin.Core.Models
{
    public class RoleUpdateRequest
    {
        public Guid RoleId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
