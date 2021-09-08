using System;

namespace Marlin.Core.Models
{
    public class UserAuthorizeRequest
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
