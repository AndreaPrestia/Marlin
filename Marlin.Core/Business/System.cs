using Marlin.Core.Common;
using Marlin.Core.Entities;
using Marlin.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Marlin.Core.Business
{
    public class System
    {
        private static readonly IStorage _storage = StorageManager.Storage;

        /// <summary>
        /// Write trace
        /// </summary>
        /// <param name="trace"></param>
        internal static void TraceWrite(Trace trace)
        {
            _storage.TraceWrite(trace);
        }

        /// <summary>
        /// Get language by id
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        internal static Language LanguageGet(string languageId)
        {
            if (string.IsNullOrEmpty(languageId))
            {
                languageId = Settings.Get("DefaultLanguageId", "en");
            }

            return _storage.LanguageGet(languageId);
        }

        /// <summary>
        /// Sign up a user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="properties">User properties</param>
        internal static void SignUp(string username, Dictionary<string, string> properties = null)
        {
            Role role = Business.Administration.RoleGet(Settings.Get("DefaultRole", "Users"));

            if (role == null)
            {
                throw new ArgumentNullException("No default role provided in database.");
            }

            IMessageHandler messageHandler = Helper.CreateInstance<IMessageHandler>(Settings.Current.MessageHandlerImplementationType);

            if (messageHandler == null)
            {
                throw new ApplicationException("No setting MessageHandlerType configured. Provide it to send correctly messages to users.");
            }

            Business.User.UserAdd(username);

            Entities.User user = _storage.UserGet(username);

            if(user == null)
            {
                throw new ArgumentNullException("User not created.");
            }

            Business.Administration.UserAuthorize(user.Id, role.Id);

            if(properties != null)
            {
                foreach(KeyValuePair<string, string> property in properties)
                {
                    _storage.UserPropertyAdd(user, property.Key, property.Value);

                    user.Properties.Add(property.Key, property.Value);
                }
            }

            Guid resetToken = Guid.NewGuid();

            _storage.UserSetResetToken(user, resetToken);

            messageHandler.Send(user, resetToken.ToString());
        }
    }
}
