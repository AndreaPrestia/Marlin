using Marlin.Core.Entities;
using System;
using System.Collections.Generic;

namespace Marlin.Core.Interfaces
{
    public interface IStorage
    {
        public void AssemblyAdd(string name);

        public void AssemblyUpdate(Assembly assembly);
        public void AssemblyDelete(Guid assemblyId);
        public bool AssemblyCanAccess(User user, string name);
        public Assembly AssemblyGet(string name);
        public List<Assembly> AssemblyList();
        public void UserAdd(string username);
        public void UserPropertyAdd(User user, string key, string value);
        public void UserPropertyUpdate(User user, string key, string value);
        public void UserPropertyDelete(User user, string key);
        public string UserPropertyGet(User user, string key);
        public void UserSetResetToken(User user, Guid resetToken);
        public void UserUpdate(Guid userId, string username);
        public List<User> UserSearch(string query, int page, int limit);
        public User UserGet(string user);
        public User UserGetByResetToken(Guid resetToken);
        public void UserDisable(Guid userId);
        public void UserEnable(Guid userId);
        public void UserDelete(Guid userId);
        public void UserAuthorize(Guid userId, Guid roleId);
        public void UserUnauthorize(Guid userId, Guid roleId);
        public void CredentialAdd(User user, string value);
        public Credential CredentialGet(User user, string value);
        public void CredentialDeleteLatest(User user);
        public List<Role> RoleList();
        public void RoleAdd(string name);
        public Role RoleGet(string role);
        public void RoleUpdate(Guid roleId, string name);
        public void RoleAddResource(Guid roleId, Guid resourceId);
        public void RoleRemoveResource(Guid roleId, Guid resourceId);
        public void RoleAddAssembly(Guid roleId, Guid assemblyId);
        public void RoleRemoveAssembly(Guid roleId, Guid assemblyId);
        public void RoleDelete(Guid roleId);
        public List<Resource> ResourceList();
        public void ResourceAdd(Resource resource);
        public Resource ResourceGet(string url, string method);
        public void ResourceUpdate(Resource resource);
        public void ResourceDelete(Guid resourceId);
        public bool ResourceIsPublic(string url, string method);
        public void TraceWrite(Trace trace);
        public void LanguageAdd(Language language);
        public void LanguageUpdate(Language language);
        public void LanguageDelete(string languageId);
        public Language LanguageGet(string languageId);
        public void TranslationAdd(Translation translation);
        public void TranslationUpdate(Translation translation);
        public void TranslationDelete(Translation translation);
        public Translation TranslationGet(string languageId, string original);
    }
}
