using Marlin.Core.Common;
using Marlin.Core.Entities;
using Marlin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text.RegularExpressions;

namespace Marlin.Core.Business
{
    public static class User
    {
        private static readonly IStorage _storage = StorageManager.Storage;

        private static NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        /// <summary>
        /// Add user to the system
        /// </summary>
        /// <param name="username"></param>
        internal static void UserAdd(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            Entities.User user = _storage.UserGet(username); 

            if(user != null)
            {
                throw new InvalidOperationException("User already registered.");
            }

            _storage.UserAdd(username);
        }

        /// <summary>
        /// Adds, updates or deletes a property for user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        internal static void UserPropertySet(Entities.User user, string key, string value)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.Id == Guid.Empty)
            {
                throw new ArgumentException(nameof(user.Id));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrEmpty(value))
            {
                _storage.UserPropertyDelete(user, key);
            }
            else
            {
                if(!string.IsNullOrEmpty(_storage.UserPropertyGet(user, key)))
                {
                    _storage.UserPropertyUpdate(user, key, value);
                }
                else
                {
                    _storage.UserPropertyAdd(user, key, value);
                }
            }
        }

        /// <summary>
        /// Updates user to the system
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        internal static void UserUpdate(Guid userId , string username)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException(nameof(userId));
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            Entities.User user = _storage.UserGet(username);

            if(user != null && user.Id != userId)
            {
                throw new InvalidOperationException("User already registered with this username.");
            }

            _storage.UserAdd(username);
        }

        /// <summary>
        /// Get list of users paginated and with a query provided
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        internal static List<Entities.User> Search(string query, int page, int limit)
        {
            if(page <= 0)
            {
                logger.Warn($"Invalid page {page} provided");
                page = 0;
            }

            if(limit < 1 || limit > 10000)
            {
                logger.Warn($"Invalid limit {limit} provided");
                limit = 10;
            }

            return _storage.UserSearch(query, page, limit);
        }
    
        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="repeat"></param>
        internal static void ChangePassword(string username ,string currentPassword, string newPassword, string repeat)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrEmpty(currentPassword))
            {
                throw new ArgumentNullException(nameof(currentPassword));
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException(nameof(newPassword));
            }

            if (string.IsNullOrEmpty(repeat))
            {
                throw new ArgumentNullException(nameof(repeat));
            }

            if (newPassword.Equals(repeat))
            {
                throw new ArgumentException("password and repeat are not the same.");
            }

            string passwordRegex = Settings.Get("PasswordRegex", @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");

            if (!Regex.IsMatch(newPassword, passwordRegex))
            {
                throw new ArgumentException("password doesn't meet complexity requested.");
            }

            Entities.User user = _storage.UserGet(username);

            if (user == null)
            {
                logger.Warn($"Change password failed for username {username}");

                throw new SecurityException("Unauthorized");
            }

            if (user.Disabled > 0)
            {
                logger.Warn($"Disabled user {user.Id} tried login");

                throw new SecurityException("Unauthorized");
            }

            currentPassword = Helper.GetSha256(currentPassword);

            Credential credential = _storage.CredentialGet(user, currentPassword);

            if (credential == null)
            {
                logger.Warn($"Change password failed for username {username} & password {currentPassword}");

                throw new SecurityException("Unauthorized");
            }

            if (credential.Deleted > 0)
            {
                logger.Warn($"Change password failed for username {username} & password {currentPassword}. Old password used.");

                throw new SecurityException("Unauthorized");
            }

            newPassword = Helper.GetSha256(newPassword);

            _storage.CredentialDeleteLatest(user);

            _storage.CredentialAdd(user, newPassword);

            logger.Info($"Added new password for user {username}");
        }
    }
}
