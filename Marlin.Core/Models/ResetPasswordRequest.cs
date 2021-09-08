using System.ComponentModel.DataAnnotations;

namespace Marlin.Core.Models
{
    public class ResetPasswordRequest
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(36)]
        public string ResetToken { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Repeat { get; set; }
    }
}
