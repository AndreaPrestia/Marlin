using Marlin.Core.Entities;
using Marlin.Core.Interfaces;
using Marlin.Core.Models;

namespace Marlin.Core.Handlers
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        LoginResponse IAuthorizationHandler.ThirdPartLogin(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
