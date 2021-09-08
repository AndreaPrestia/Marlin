using System.ComponentModel.DataAnnotations;

namespace Marlin.Core.Models
{
    public class LoginRequest
    {
        [Required(AllowEmptyStrings =false)]
        [MaxLength(200)]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(200)]
        public string Password { get; set; }
    }
}
