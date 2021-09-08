using Marlin.Core.Entities;

namespace Marlin.Core.Models
{
    public class LoginResponse
    {
        public User User { get; set; }
        public string Bearer { get; set; }
    }
}
