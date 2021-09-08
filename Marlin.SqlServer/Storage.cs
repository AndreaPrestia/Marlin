using Marlin.Core;
using Marlin.Core.Common;
using Marlin.Core.Entities;
using Marlin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Marlin.SqlServer
{
    public class Storage : IStorage
    {
        public void AssemblyAdd(string name)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_AssemblyAdd", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@name", Value = name.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool AssemblyCanAccess(User user, string name)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_AssemblyCanAccess", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = user.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@name", Value = name.Trim() });

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }

        public void AssemblyDelete(Guid assemblyId)
        {
           using(SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using(SqlCommand command = new SqlCommand("Marlin_AssemblyDelete", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@assembly", Value = assemblyId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public Assembly AssemblyGet(string name)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_AssemblyGet", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@name", Value = name.Trim() });


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Assembly assembly = null;

                        if (reader.Read())
                        {
                            assembly = new Assembly();

                            assembly.Id = reader.Get<Guid>("Id");
                            assembly.Name = reader.Get<string>("Name");
                        }

                        return assembly;
                    }
                }
            }
        }

        public List<Assembly> AssemblyList()
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_AssemblyGet", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Assembly> assemblies = new List<Assembly>();

                        if (reader.Read())
                        {
                            Assembly assembly = new Assembly();

                            assembly.Id = reader.Get<Guid>("Id");
                            assembly.Name = reader.Get<string>("Name");

                            assemblies.Add(assembly);
                        }

                        return assemblies;
                    }
                }
            }
        }

        public void AssemblyUpdate(Assembly assembly)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_AssemblyUpdate", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@assembly", Value = assembly.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@name", Value = assembly.Name.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void CredentialAdd(User user, string value)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_CredentialAdd", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = user.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@value", Value = value.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void CredentialDeleteLatest(User user)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_CredentialDeleteLatest", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = user.Id });

                    command.ExecuteNonQuery();
                }
            }
        }

        public Credential CredentialGet(User user, string value)
        {
            Credential credential = null;

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_CredentialGet", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = user.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@value", Value = value.Trim() });

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            credential = new Credential();

                            credential.Id = reader.Get<Guid>("Id");
                            credential.Created = Helper.GetUnixTimestamp(reader.Get<DateTime>("Created"));
                            credential.Deleted = Helper.GetUnixTimestamp(reader.Get<DateTime>("Deleted"));
                        }
                    }
                }
            }

            return credential;
        }

        public void LanguageAdd(Language language)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_LanguageAdd", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@language", Value = language.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@name", Value = language.Name.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void LanguageDelete(string languageId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_LanguageDelete", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@language", Value = languageId.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public Language LanguageGet(string languageId)
        {
            Language language = null;

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_LanguageDelete", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@language", Value = languageId.Trim() });

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (language == null)
                            {
                                language = new Language();
                                language.Translations = new List<Translation>();

                                language.Id = reader.Get<string>("Id");
                                language.Name = reader.Get<string>("Name");
                            }

                            string original = reader.Get<string>("Original");

                            if (!string.IsNullOrEmpty(original) && !language.Translations.Any(x => x.Original == original))
                            {
                                Translation translation = new Translation();

                                translation.LanguageId = language.Id;
                                translation.Original = original;
                                translation.Translated = reader.Get<string>("Translated");

                                language.Translations.Add(translation);
                            }
                        }
                    }
                }
            }

            return language;
        }

        public void LanguageUpdate(Language language)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_LanguageUpdate", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@language", Value = language.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@name", Value = language.Name.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void ResourceAdd(Resource resource)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_ResourceAdd", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@url", Value = resource.Url.Trim() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@method", Value = resource.Method.Trim() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@isPublic", Value = resource.IsPublic });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@order", Value = resource.Order });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@title", Value = !string.IsNullOrEmpty(resource.Title) ? resource.Title : (object)DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@label", Value = !string.IsNullOrEmpty(resource.Label) ? resource.Label : (object)DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@parent", Value = resource.ParentId != Guid.Empty ? resource.ParentId : (object)DBNull.Value });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void ResourceDelete(Guid resourceId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_ResourceDelete", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@resource", Value = resourceId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public Resource ResourceGet(string url, string method)
        {
            Resource resource = null;

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_ResourceGet", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@url", Value = url.Trim() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@method", Value = method.Trim() });

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            resource = new Resource();

                            resource.Id = reader.Get<Guid>("Id");
                            resource.IsPublic = reader.Get<bool>("IsPublic");
                            resource.Label = reader.Get<string>("Label");
                            resource.Method = reader.Get<string>("Method");
                            resource.Order = reader.Get<int>("Order");
                            resource.ParentId = reader.Get<Guid>("ParentId");
                            resource.Title = reader.Get<string>("Title");
                            resource.Url = reader.Get<string>("Url");
                        }
                    }
                }
            }

            return resource;
        }

        public bool ResourceIsPublic(string url, string method)
        {
            bool isPublic = false;

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_ResourceIsPublic", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@url", Value = url.Trim() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@method", Value = method.Trim() });

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isPublic = reader.Get<bool>("IsPublic");
                        }
                    }
                }
            }

            return isPublic;
        }

        public List<Resource> ResourceList()
        {
            List<Resource> resources = new List<Resource>();

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_ResourceList", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Resource resource = new Resource();

                            resource.Id = reader.Get<Guid>("Id");
                            resource.IsPublic = reader.Get<bool>("IsPublic");
                            resource.Label = reader.Get<string>("Label");
                            resource.Method = reader.Get<string>("Method");
                            resource.Order = reader.Get<int>("Order");
                            resource.ParentId = reader.Get<Guid>("ParentId");
                            resource.Title = reader.Get<string>("Title");
                            resource.Url = reader.Get<string>("Url");

                            resources.Add(resource);
                        }
                    }
                }
            }

            if (resources != null && resources.Count > 0)
            {
                resources = resources.BuildResourceTree();
            }

            return resources;
        }

        public void ResourceUpdate(Resource resource)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_ResourceUpdate", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@resource", Value = resource.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@url", Value = resource.Url.Trim() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@method", Value = resource.Method.Trim() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@isPublic", Value = resource.IsPublic });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@order", Value = resource.Order });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@title", Value = !string.IsNullOrEmpty(resource.Title) ? resource.Title : (object)DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@label", Value = !string.IsNullOrEmpty(resource.Label) ? resource.Label : (object)DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@parent", Value = resource.ParentId != Guid.Empty ? resource.ParentId : (object)DBNull.Value });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void RoleAdd(string name)
        {
            Guid roleId = Guid.Empty;

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_RoleAdd", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@name", Value = name.Trim() });

                    command.ExecuteNonQuery();
                }
            }

        }

        public void RoleAddAssembly(Guid roleId, Guid assemblyId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_RoleAddAssembly", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@role", Value = roleId });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@assembly", Value = assemblyId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void RoleAddResource(Guid roleId, Guid resourceId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_RoleAddResource", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@role", Value = roleId });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@resource", Value = resourceId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void RoleDelete(Guid roleId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_RoleDelete", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@role", Value = roleId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public Role RoleGet(string role)
        {
            Role r = null;

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_RoleGet", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@role", Value = role.Trim() });

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (r == null)
                            {
                                r = new Role();

                                r.Id = reader.Get<Guid>("RoleId");
                                r.Name = reader.Get<string>("Role");
                                r.Resources = new List<Resource>();
                                r.Assemblies = new List<Assembly>();
                            }

                            Guid resourceId = reader.Get<Guid>("ResourceId");
                            Guid assemblyId = reader.Get<Guid>("AssemblyId");

                            if (resourceId != Guid.Empty && !r.Resources.Any(x => x.Id == resourceId))
                            {
                                Resource resource = new Resource();

                                resource.Id = resourceId;
                                resource.IsPublic = reader.Get<bool>("IsPublic");
                                resource.Label = reader.Get<string>("Label");
                                resource.Method = reader.Get<string>("Method");
                                resource.Order = reader.Get<int>("Order");
                                resource.ParentId = reader.Get<Guid>("ParentId");
                                resource.Title = reader.Get<string>("Title");
                                resource.Url = reader.Get<string>("Url");

                                r.Resources.Add(resource);
                            }

                            if (assemblyId != Guid.Empty && !r.Assemblies.Any(x => x.Id == assemblyId))
                            {
                                Assembly assembly = new Assembly();

                                assembly.Id = reader.Get<Guid>("AssemblyId");
                                assembly.Name = reader.Get<string>("Assembly");

                                r.Assemblies.Add(assembly);
                            }
                        }
                    }
                }
            }

            if (r != null && r.Resources.Count > 0)
            {
                r.Resources = r.Resources.BuildResourceTree();
            }

            return r;
        }

        public List<Role> RoleList()
        {
            List<Role> roles = new List<Role>();

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_RoleList", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Guid roleId = reader.Get<Guid>("RoleId");

                            Role role = roles.Where(x => x.Id == roleId).FirstOrDefault();

                            if (role == null)
                            {
                                role = new Role();

                                role.Id = roleId;
                                role.Name = reader.Get<string>("Role");
                                role.Resources = new List<Resource>();
                                role.Assemblies = new List<Assembly>();

                                roles.Add(role);
                            }

                            Guid resourceId = reader.Get<Guid>("ResourceId");
                            Guid assemblyId = reader.Get<Guid>("AssemblyId");

                            if (resourceId != Guid.Empty && !role.Resources.Any(x => x.Id == resourceId))
                            {
                                Resource resource = new Resource();

                                resource.Id = resourceId;
                                resource.IsPublic = reader.Get<bool>("IsPublic");
                                resource.Label = reader.Get<string>("Label");
                                resource.Method = reader.Get<string>("Method");
                                resource.Order = reader.Get<int>("Order");
                                resource.ParentId = reader.Get<Guid>("ParentId");
                                resource.Title = reader.Get<string>("Title");
                                resource.Url = reader.Get<string>("Url");

                                role.Resources.Add(resource);
                            }

                            if (assemblyId != Guid.Empty && !role.Assemblies.Any(x => x.Id == assemblyId))
                            {
                                Assembly assembly = new Assembly();

                                assembly.Id = reader.Get<Guid>("AssemblyId");
                                assembly.Name = reader.Get<string>("Assembly");

                                role.Assemblies.Add(assembly);
                            }
                        }
                    }
                }
            }

            if (roles.Count > 0)
            {
                foreach (Role role in roles)
                {
                    if (role.Resources.Count > 0)
                    {
                        role.Resources = role.Resources.BuildResourceTree();
                    }
                }
            }

            return roles;
        }

        public void RoleRemoveAssembly(Guid roleId, Guid assemblyId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_RoleRemoveAssembly", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@role", Value = roleId });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@assembly", Value = assemblyId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void RoleRemoveResource(Guid roleId, Guid resourceId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_RoleRemoveResource", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@role", Value = roleId });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@resource", Value = resourceId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void RoleUpdate(Guid roleId, string name)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_RoleUpdate", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@role", Value = roleId });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@name", Value = name.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void TraceWrite(Trace trace)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_TraceWrite", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@className", Value = trace.ClassName });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@method", Value = trace.Method });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@hostname", Value = trace.Hostname });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@clientIp", Value = trace.ClientIp });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@url", Value = trace.Url });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@millis", Value = trace.Millis });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@username", Value = !string.IsNullOrEmpty(trace.Username) ? trace.Username : (object)DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@error", Value = !string.IsNullOrEmpty(trace.Error) ? trace.Error : (object)DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@payload", Value = !string.IsNullOrEmpty(trace.Payload) ? trace.Payload : (object)DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@query", Value = !string.IsNullOrEmpty(trace.Query) ? trace.Query : (object)DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@message", Value = !string.IsNullOrEmpty(trace.Message) ? trace.Message : (object)DBNull.Value });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void TranslationAdd(Translation translation)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_TranslationAdd", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@language", Value = translation.LanguageId.ToString() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@original", Value = translation.Original.Trim() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@translated", Value = translation.Translated.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void TranslationDelete(Translation translation)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_TranslationDelete", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@language", Value = translation.LanguageId.ToString() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@original", Value = translation.Original.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public Translation TranslationGet(string languageId, string original)
        {
            Translation translation = null;
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_TranslationGet", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@language", Value = languageId.Trim() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@original", Value = original.Trim() });

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            translation = new Translation();

                            translation.LanguageId = languageId;
                            translation.Original = original;
                            translation.Translated = reader.Get<string>("Translated");
                        }
                    }
                }
            }

            return translation;
        }

        public void TranslationUpdate(Translation translation)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_TranslationUpdate", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@language", Value = translation.LanguageId.ToString() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@original", Value = translation.Original.Trim() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@translated", Value = translation.Translated.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UserAdd(string username)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserAdd", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@username", Value = username.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UserAuthorize(Guid userId, Guid roleId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserAuthorize", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@role", Value = roleId });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = userId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UserDelete(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserDelete", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = userId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UserDisable(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserDisable", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = userId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UserEnable(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserEnable", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = userId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public User UserGet(string user)
        {
            User dbUser = null;

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserGet", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = user.Trim() });

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (user == null)
                            {
                                dbUser = new User();

                                dbUser.Id = reader.Get<Guid>("Id");
                                dbUser.Username = reader.Get<string>("Username");
                                dbUser.Created = Helper.GetUnixTimestamp(reader.Get<DateTime>("Created"));
                                dbUser.Properties = new Dictionary<string, string>();
                                dbUser.Roles = new List<Role>();
                            }

                            string key = reader.Get<string>("Key");

                            if (!string.IsNullOrEmpty(key) && !dbUser.Properties.ContainsKey(key))
                            {
                                dbUser.Properties.Add(key, reader.Get<string>("Value"));
                            }

                            string roleName = reader.Get<string>("Role");

                            Role role = dbUser.Roles.Where(x => x.Name == roleName).FirstOrDefault();

                            if (!string.IsNullOrEmpty(roleName) && role == null)
                            {
                                role = new Role();

                                role.Name = roleName;

                                role.Resources = new List<Resource>();
                                role.Assemblies = new List<Assembly>();

                                dbUser.Roles.Add(role);
                            }

                            string url = reader.Get<string>("Url");
                            string method = reader.Get<string>("Method");

                            if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(method) && !role.Resources.Any(x => x.Url == url && x.Method == method))
                            {
                                Resource resource = new Resource();

                                resource.IsPublic = reader.Get<bool>("IsPublic");
                                resource.Label = reader.Get<string>("Label");
                                resource.Method = method;
                                resource.Order = reader.Get<int>("Order");
                                resource.ParentId = reader.Get<Guid>("ParentId");
                                resource.Title = reader.Get<string>("Title");
                                resource.Url = reader.Get<string>("Url");

                                role.Resources.Add(resource);
                            }

                            string assemblyName = reader.Get<string>("Assembly");

                            if (!string.IsNullOrEmpty(assemblyName) && !role.Assemblies.Any(x => x.Name == assemblyName))
                            {
                                Assembly assembly = new Assembly();

                                assembly.Name = assemblyName;

                                role.Assemblies.Add(assembly);
                            }
                        }
                    }
                }
            }

            return dbUser;
        }

        public User UserGetByResetToken(Guid resetToken)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserGetByResetToken", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@resetToken", Value = resetToken });

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (user == null)
                            {
                                user = new User();

                                user.Id = reader.Get<Guid>("Id");
                                user.Username = reader.Get<string>("Username");
                                user.Created = Helper.GetUnixTimestamp(reader.Get<DateTime>("Created"));
                                user.Disabled = Helper.GetUnixTimestamp(reader.Get<DateTime>("Deleted"));
                                user.Properties = new Dictionary<string, string>();
                            }

                            string key = reader.Get<string>("Key");

                            if (!string.IsNullOrEmpty(key) && !user.Properties.ContainsKey(key))
                            {
                                user.Properties.Add(key, reader.Get<string>("Value"));
                            }
                        }
                    }
                }
            }

            return user;
        }

        public void UserPropertyAdd(User user, string key, string value)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserPropertyAdd", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = user.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@key", Value = key.Trim() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@value", Value = value.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UserPropertyDelete(User user, string key)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserPropertyDelete", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = user.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@key", Value = key.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public string UserPropertyGet(User user, string key)
        {
            string value = string.Empty;

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserPropertyGet", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = user.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@key", Value = key.Trim() });

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            value = reader.Get<string>("Value");
                        }
                    }
                }
            }

            return value;
        }

        public void UserPropertyUpdate(User user, string key, string value)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserPropertyUpdate", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = user.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@key", Value = key.Trim() });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@value", Value = value.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<User> UserSearch(string query, int page, int limit)
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserGetByResetToken", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@query", Value = !string.IsNullOrEmpty(query) ? query.Trim() : (Object)DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@page", Value = page });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@limit", Value = limit });

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Guid userId = reader.Get<Guid>("Id");

                            User user = users.Where(x => x.Id == userId).FirstOrDefault();

                            if (user == null)
                            {
                                user = new User();

                                user.Id = reader.Get<Guid>("Id");
                                user.Username = reader.Get<string>("Username");
                                user.Created = Helper.GetUnixTimestamp(reader.Get<DateTime>("Created"));
                                user.Properties = new Dictionary<string, string>();

                                users.Add(user);
                            }

                            string key = reader.Get<string>("Key");

                            if (!string.IsNullOrEmpty(key) && !user.Properties.ContainsKey(key))
                            {
                                user.Properties.Add(key, reader.Get<string>("Value"));
                            }
                        }
                    }
                }
            }

            return users;
        }

        public void UserSetResetToken(User user, Guid resetToken)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserSetResetToken", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = user.Id });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@resetToken", Value = resetToken });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UserUnauthorize(Guid userId, Guid roleId)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserUnauthorize", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@role", Value = roleId });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = userId });

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UserUpdate(Guid userId, string username)
        {
            using (SqlConnection connection = new SqlConnection(Settings.Current.StorageSource))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Marlin_UserUpdate", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@user", Value = userId });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@username", Value = username.Trim() });

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
