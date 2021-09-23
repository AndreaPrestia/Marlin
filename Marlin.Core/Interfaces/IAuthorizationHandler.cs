using Marlin.Core.Entities;
using Marlin.Core.Models;

namespace Marlin.Core.Interfaces
{
    public interface IAuthorizationHandler
    {
        public User Login(string username, string password);

        public string GenerateCredential(User user);

        public User Login(string username);
    }
}
