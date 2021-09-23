using Marlin.Core.Common;
using Marlin.Core.Entities;
using Marlin.Core.Interfaces;
using Marlin.Core.Models;
using System;
using System.Collections.Generic;
using System.Security;

namespace Marlin.Core.Business
{
    public static class Authorization
    {
        private static NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        private static readonly IStorage _storage = StorageManager.Storage;

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

            return _storage.ResourceCanAccess(Context.Current.User.Id, url.ToLower(), method.ToUpper());
        }

        /// <summary>
        /// Login procedure (it contains the current implementation of IAuthorizationHandler)
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

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            IAuthorizationHandler authorizationHandler = Helper.CreateInstance<IAuthorizationHandler>(Settings.Current.AuthorizationHandlerImplementationType);

            password = Helper.GetSha256(password);

            LoginResponse response = new LoginResponse();

            response.User = authorizationHandler.Login(username, password);

            if(response.User == null)
            {
                logger.Warn($"Login failed for username {username}");

                throw new SecurityException("Login failed");
            }

            if(response.User.Disabled > 0)
            {
                logger.Warn($"Disabled user {response.User.Id} tried login");

                throw new SecurityException("Login failed");
            }

            response.Bearer = Helper.GetBearer(password, response.User);

            return response;
        }

        /// <summary>
        /// Check existance of user and send credentials with the implementation of IAuthorizationHandler and IMessageHandler
        /// </summary>
        /// <param name="user"></param>
        internal static void Login(string username)
        {
            if(string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            IAuthorizationHandler authorizationHandler = Helper.CreateInstance<IAuthorizationHandler>(Settings.Current.AuthorizationHandlerImplementationType);

            IMessageHandler messageHandler = Helper.CreateInstance<IMessageHandler>(Settings.Current.MessageHandlerImplementationType);

            Entities.User user = authorizationHandler.Login(username);

            if (user == null)
            {
                logger.Warn($"Login failed for username {username}");

                throw new SecurityException("Login failed");
            }

            string credential = authorizationHandler.GenerateCredential(user);

            string hashedCredential = Helper.GetSha256(credential);

            _storage.CredentialAdd(user, hashedCredential);

            messageHandler.Send(user, credential);
        }

        /// <summary>
        /// Get user resources
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static List<Resource> UserResources(Entities.User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if(user.Id == Guid.Empty)
            {
                throw new ArgumentException(nameof(user.Id));
            }

            if (string.IsNullOrEmpty(user.Username))
            {
                throw new ArgumentNullException(nameof(user.Username));
            }

            return _storage.UserResources(user.Username);
        }

        /// <summary>
        /// Get a bearer token for user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal static string GetBearer(Entities.User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.Id == Guid.Empty)
            {
                throw new ArgumentException(nameof(user.Id));
            }

            Credential credential = _storage.CredentialGet(user);

            if(credential == null)
            {
                logger.Warn($"User {user.Id} tried to generate a new bearer, but not found valid credentials to encrypt its token.");

                throw new UnauthorizedAccessException("Unauthorized");
            }

            string token = Helper.GetBearer(credential.Value, Context.Current.User);

            return token;
        }
    }
}
