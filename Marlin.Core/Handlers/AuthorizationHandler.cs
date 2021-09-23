using Marlin.Core.Common;
using Marlin.Core.Entities;
using Marlin.Core.Interfaces;
using System.Security;

namespace Marlin.Core.Handlers
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        private static readonly IStorage _storage = StorageManager.Storage;

        public string GenerateCredential(User user)
        {
            return Helper.GetRandomString();
        }

        public User Login(string username, string password)
        {
            User user = _storage.UserGet(username);

            if(user == null)
            {
                throw new SecurityException("Login failed");
            }

            Credential credential = _storage.CredentialGet(user, password);

            if(credential == null)
            {
                throw new SecurityException("Login failed");
            }

            return user;
        }

        public User Login(string username)
        {
            return _storage.UserGet(username);
        }
    }
}
