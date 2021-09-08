using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Marlin.Core.Models
{
    public class SignUpRequest
    {   
        [Required(AllowEmptyStrings = false)]
        [MaxLength(200)]
        public string Username { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}
