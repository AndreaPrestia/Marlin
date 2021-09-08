using Marlin.Core.Entities;
using Marlin.Core.Models;

namespace Marlin.Core.Interfaces
{
    public interface IAuthorizationHandler
    {
        public LoginResponse ThirdPartLogin(User user);
    }
}
