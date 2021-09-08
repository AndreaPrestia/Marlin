using System.ComponentModel.DataAnnotations;

namespace Marlin.Core.Models
{
    public class PasswordRequest
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(200)]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Current { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string New { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string Repeat { get; set; }
    }
}
