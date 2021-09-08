using Marlin.Core.Common;
using Marlin.Core.Entities;
using Marlin.Core.Exceptions;
using Marlin.Core.Interfaces;
using Marlin.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Marlin.Core.Business
{
    public static class Authorization
    {
        private static NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        private static readonly IStorage _storage = StorageManager.Storage;

        /// <summary>
        /// Checks if a user can access to an assembly 
        /// </summary>
        /// <param name="user">User to chec</param>
        /// <param name="name">Assembly name</param>
        /// <returns></returns>
        public static bool CanAccessAssembly(Entities.User user, string name)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.Id == Guid.Empty)
            {
                throw new ArgumentException(nameof(user.Id));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _storage.AssemblyCanAccess(user, name);
        }

        /// <summary>
        /// Checks if a resource is public
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool IsPublicResource(string url, string method)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException(nameof(method));
            }

            return _storage.ResourceIsPublic(url, method);
        }

        /// <summary>
        /// Checks if current user can access to resource provided
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool CanAccessResource(string url, string method)
        {
            if(Context.Current.User == null)
            {
                throw new SecurityException("No user context");
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException(nameof(method));
            }

            return Context.Current.User.Roles.Any(x => x.Resources.Any(y => y.Url.Equals(url.ToLower()) && y.Method.Equals(method.ToUpper())));
        }

        /// <summary>
        /// Login procedure (it contains the third part login interface implementations, if active on your configuration)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal static LoginResponse Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            LoginResponse response = new LoginResponse();

            response.User = _storage.UserGet(username);

            if(response.User == null)
            {
                logger.Warn($"Login failed for username {username}");

                throw new SecurityException("Unauthorized");
            }

            if(response.User.Disabled > 0)
            {
                logger.Warn($"Disabled user {response.User.Id} tried login");

                throw new SecurityException("Unauthorized");
            }

            bool thirdPartLogin = Settings.Get("ThirdPartLogin", false);

            if (thirdPartLogin)
            {
                IAuthorizationHandler authorizationHandler = Helper.CreateInstance<IAuthorizationHandler>(Settings.Current.AuthorizationHandlerImplementationType);

                if(authorizationHandler == null)
                {
                    throw new ApplicationException("No ThirdPartLoginType setting configured. Cannot do the login.");
                }

                return authorizationHandler.ThirdPartLogin(response.User);
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            password = Helper.GetSha256(password);

            Credential credential = _storage.CredentialGet(response.User, password);

            if(credential == null)
            {
                logger.Warn($"Login failed for username {username} & password {password}");

                throw new SecurityException("Unauthorized");
            }

            if(credential.Deleted > 0)
            {
                logger.Warn($"Login failed for username {username} & password {password}. Old password used.");

                throw new SecurityException("Unauthorized");
            }

            int passwordExpirationDays = Settings.Get<int>("PasswordExpirationDays", 30);

            if(Helper.GetFromUnixTimestamp(credential.Created).AddDays(passwordExpirationDays) > DateTime.Now)
            {
                throw new PasswordExpiredException("Password expired");
            }

            response.Bearer = Helper.GetBearer(Settings.Get<string>("ServerSecret"), response.User);

            return response;
        }

        /// <summary>
        /// Ask reset password for specified user
        /// </summary>
        /// <param name="username"></param>
        internal static void ResetPassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            IMessageHandler messageHandler = Helper.CreateInstance<IMessageHandler>(Settings.Current.MessageHandlerImplementationType);

            if(messageHandler == null)
            {
                throw new ApplicationException("No setting MessageHandlerType configured. Provide it to send correctly messages to users.");
            }

            Entities.User user = _storage.UserGet(username);

            if(user == null)
            {
                logger.Warn($"Tried reset password on username {username}");
                return;
            }

            if(user.Disabled > 0)
            {
                logger.Warn($"Tried reset password on disabled user {user.Id}");
                return;
            }

            Guid resetToken = Guid.NewGuid();

            _storage.UserSetResetToken(user, resetToken);

            messageHandler.Send(user, resetToken.ToString());
        }

        /// <summary>
        /// Resets the password for the user specified by token
        /// </summary>
        /// <param name="resetToken"></param>
        /// <param name="password"></param>
        /// <param name="repeat"></param>
        internal static void ResetPassword(Guid resetToken, string password, string repeat)
        {
            if(resetToken == Guid.Empty)
            {
                throw new ArgumentException(nameof(resetToken));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrEmpty(repeat))
            {
                throw new ArgumentNullException(nameof(repeat));
            }

            if (password.Equals(repeat))
            {
                throw new ArgumentException("password and repeat are not the same.");
            }

            string passwordRegex = Settings.Get("PasswordRegex", @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");

            if(!Regex.IsMatch(password, passwordRegex))
            {
                throw new ArgumentException("password doesn't meet complexity requested.");
            }

            Entities.User user = _storage.UserGetByResetToken(resetToken);

            if(user == null)
            {
                logger.Warn($"User not found with resetToken {resetToken}");

                throw new SecurityException("Invalid request");
            }

            if(user.Disabled > 0)
            {
                logger.Warn($"User {user.Id} disabled.");

                throw new SecurityException("Invalid request");
            }

            password = Helper.GetSha256(password);

            Credential credential = _storage.CredentialGet(user, password);

            if(credential != null)
            {
                int passwordAlreadyUsedDays = Settings.Get("PasswordAlreadyUsed", 90);

                if(Helper.GetFromUnixTimestamp(credential.Created).AddDays(passwordAlreadyUsedDays) > DateTime.Now)
                {
                    throw new InvalidOperationException($"Your password is already used in the last {passwordAlreadyUsedDays} days.");
                }
            }

            _storage.CredentialDeleteLatest(user);

            _storage.CredentialAdd(user, Helper.GetSha256(password));
        }
    }
}
