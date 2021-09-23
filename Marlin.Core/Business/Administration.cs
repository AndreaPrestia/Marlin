using Marlin.Core.Entities;
using Marlin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marlin.Core.Business
{
    public static class Administration
    {
        private static readonly IStorage _storage = StorageManager.Storage;

        #region Role

        /// <summary>
        /// List of platform roles
        /// </summary>
        /// <returns></returns>
        internal static List<Role> RoleList() => _storage.RoleList();

        /// <summary>
        /// Get a role.
        /// </summary>
        /// <param name="role">Can be name or Guid as string</param>
        /// <returns></returns>
        internal static Role RoleGet(string role) => _storage.RoleGet(role);

        /// <summary>
        /// Adds a role
        /// </summary>
        /// <param name="name">Role name</param>
        internal static void RoleAdd(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Role role = _storage.RoleGet(name);

            if (role != null)
            {
                throw new InvalidOperationException("Role already registered");
            }

            _storage.RoleAdd(name);
        }

        /// <summary>
        /// Updates a role name
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="name"></param>
        internal static void RoleUpdate(Guid roleId, string name)
        {
            if (roleId == Guid.Empty)
            {
                throw new ArgumentException(nameof(roleId));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Role role = _storage.RoleGet(name);

            if (role != null && role.Id != roleId)
            {
                throw new InvalidOperationException("Role already registered");
            }

            _storage.RoleUpdate(roleId, name);
        }

        /// <summary>
        /// Adds a resource to role
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="resourceId"></param>
        internal static void RoleAddResource(Guid roleId, Guid resourceId)
        {
            if (roleId == Guid.Empty)
            {
                throw new ArgumentException(nameof(roleId));
            }

            if (resourceId == Guid.Empty)
            {
                throw new ArgumentException(nameof(resourceId));
            }

            _storage.RoleAddResource(roleId, resourceId);
        }

        /// <summary>
        /// Remove a resource from role
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="resourceId"></param>
        internal static void RoleRemoveResource(Guid roleId, Guid resourceId)
        {
            if (roleId == Guid.Empty)
            {
                throw new ArgumentException(nameof(roleId));
            }

            if (resourceId == Guid.Empty)
            {
                throw new ArgumentException(nameof(resourceId));
            }

            _storage.RoleRemoveResource(roleId, resourceId);
        }

        /// <summary>
        /// Deletes a role
        /// </summary>
        /// <param name="roleId"></param>
        internal static void RoleDelete(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                throw new ArgumentException(nameof(roleId));
            }

            _storage.RoleDelete(roleId);
        }

        #endregion

        #region Resource

        /// <summary>
        /// Returns resource list
        /// </summary>
        /// <returns></returns>
        internal static List<Resource> ResourceList() => _storage.ResourceList();

        /// <summary>
        /// Adds a resource
        /// </summary>
        /// <param name="resource"></param>
        internal static void ResourceAdd(Resource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            if (string.IsNullOrEmpty(resource.Url))
            {
                throw new ArgumentNullException(nameof(resource.Url));
            }

            if (string.IsNullOrEmpty(resource.Method))
            {
                throw new ArgumentNullException(nameof(resource.Method));
            }

            if (resource.Order < 0)
            {
                throw new ArgumentException(nameof(resource.Order));
            }

            if (_storage.ResourceGet(resource.Url, resource.Method) != null)
            {
                throw new InvalidOperationException("Resource already registered.");
            }

            _storage.ResourceAdd(resource);
        }

        /// <summary>
        /// Get a resource by url and method
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static Resource ResourceGet(string url, string method)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException(nameof(method));
            }

            return _storage.ResourceGet(url, method);
        }

        /// <summary>
        /// Updates a resource
        /// </summary>
        /// <param name="resource"></param>
        internal static void ResourceUpdate(Resource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            if (resource.Id == Guid.Empty)
            {
                throw new ArgumentException(nameof(resource.Id));
            }

            if (string.IsNullOrEmpty(resource.Url))
            {
                throw new ArgumentNullException(nameof(resource.Url));
            }

            if (string.IsNullOrEmpty(resource.Method))
            {
                throw new ArgumentNullException(nameof(resource.Method));
            }

            if (resource.Order < 0)
            {
                throw new ArgumentException(nameof(resource.Order));
            }

            Resource databaseResource = _storage.ResourceGet(resource.Url, resource.Method);

            if (databaseResource != null && databaseResource.Id != resource.Id)
            {
                throw new InvalidOperationException("Resource already registered.");
            }

            _storage.ResourceUpdate(resource);
        }

        /// <summary>
        /// Deletes a resource
        /// </summary>
        /// <param name="resourceId"></param>
        internal static void ResourceDelete(Guid resourceId)
        {
            if (resourceId == Guid.Empty)
            {
                throw new ArgumentException(nameof(resourceId));
            }

            _storage.ResourceDelete(resourceId);
        }

        #endregion

        #region User
        /// <summary>
        /// Disable a user. Cannot access.
        /// </summary>
        /// <param name="userId"></param>
        internal static void UserDisable(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException(nameof(userId));
            }

            if(Context.Current.User.Id == userId)
            {
                throw new InvalidOperationException("You cannot disable yourself :)");
            }

            _storage.UserDisable(userId);
        }

        /// <summary>
        /// Enable a user. Cannot access.
        /// </summary>
        /// <param name="userId"></param>
        internal static void UserEnable(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException(nameof(userId));
            }

            if (Context.Current.User.Id == userId)
            {
                throw new InvalidOperationException("You cannot enable yourself :)");
            }

            _storage.UserEnable(userId);
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="userId"></param>
        internal static void UserDelete(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException(nameof(userId));
            }

            if (Context.Current.User.Id == userId)
            {
                throw new InvalidOperationException("You cannot delete yourself :)");
            }

            _storage.UserDelete(userId);
        }

        /// <summary>
        /// Authorize a user on a role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        internal static void UserAuthorize(Guid userId, Guid roleId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException(nameof(userId));
            }

            if (roleId == Guid.Empty)
            {
                throw new ArgumentException(nameof(roleId));
            }

            _storage.UserAuthorize(userId, roleId);
        }

        /// <summary>
        /// Unauthorize a user on a role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        internal static void UserUnauthorize(Guid userId, Guid roleId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException(nameof(userId));
            }

            if (roleId == Guid.Empty)
            {
                throw new ArgumentException(nameof(roleId));
            }

            if (Context.Current.User.Id == userId && Context.Current.User.Roles.Any(x => x.Id == roleId && x.Name == Settings.Get("AdministrationRoleName", "Administrators")))
            {
                throw new InvalidOperationException("You cannot erase your role while performing an operation related :)");
            }

            _storage.UserUnauthorize(userId, roleId);
        }

        #endregion

        #region Language
        /// <summary>
        /// Adds a language
        /// </summary>
        /// <param name="language"></param>
        internal static void LanguageAdd(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            if (string.IsNullOrEmpty(language.Id))
            {
                throw new ArgumentNullException(nameof(language.Id));
            }

            if (string.IsNullOrEmpty(language.Name))
            {
                throw new ArgumentNullException(nameof(language.Name));
            }

            Language dbLanguage = _storage.LanguageGet(language.Id);

            if (dbLanguage != null)
            {
                throw new InvalidOperationException("Language already registered.");
            }

            _storage.LanguageAdd(language);
        }

        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="language"></param>
        internal static void LanguageUpdate(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            if (string.IsNullOrEmpty(language.Id))
            {
                throw new ArgumentNullException(nameof(language.Id));
            }

            if (string.IsNullOrEmpty(language.Name))
            {
                throw new ArgumentNullException(nameof(language.Name));
            }

            Language dbLanguage = _storage.LanguageGet(language.Id);

            if (dbLanguage != null && dbLanguage.Id != language.Id)
            {
                throw new InvalidOperationException("Language already registered.");
            }

            _storage.LanguageUpdate(language);
        }

        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <param name="language"></param>
        internal static void LanguageDelete(string languageId)
        {
            if (string.IsNullOrEmpty(languageId))
            {
                throw new ArgumentNullException(nameof(languageId));
            }

            _storage.LanguageDelete(languageId);
        }

        /// <summary>
        /// Add, updates or deletes a translation
        /// </summary>
        /// <param name="translation"></param>
        internal static void TranslationSet(Translation translation)
        {
            if (translation == null)
            {
                throw new ArgumentNullException(nameof(translation));
            }

            if (string.IsNullOrEmpty(translation.LanguageId))
            {
                throw new ArgumentNullException(nameof(translation.LanguageId));
            }

            if (string.IsNullOrEmpty(translation.Original))
            {
                throw new ArgumentNullException(nameof(translation.Original));
            }

            if (string.IsNullOrEmpty(translation.Translated))
            {
                _storage.TranslationDelete(translation);
            }
            else
            {
                if (_storage.TranslationGet(translation.LanguageId, translation.Original) != null)
                {
                    _storage.TranslationUpdate(new Translation() { LanguageId = translation.LanguageId, Original = translation.Original, Translated = translation.Translated });
                }
                else
                {
                    _storage.TranslationAdd(new Translation() { LanguageId = translation.LanguageId, Original = translation.Original, Translated = translation.Translated });
                }
            }
        }

        #endregion
    }
}
