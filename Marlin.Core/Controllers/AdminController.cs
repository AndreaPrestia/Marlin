using Marlin.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Marlin.Core.Controllers
{
    [Produces(ContentType.ApplicationJson)]
    [ApiController]
    [Route("admin")]
    public class AdminController : SecureController
    {
        #region Role
        /// <summary>
        /// Available roles 
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(RolesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("roles.api")]
        public IActionResult Roles()
        {
            return GetOutput(new RolesResponse()
            {
                Roles = Business.Administration.RoleList()
            });
        }

        /// <summary>
        /// Get role by id or name
        /// <param name="role"/>Role id or name</param>
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(RolesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("role.api")]
        public IActionResult Role(string role)
        {
            return GetOutput(new RoleResponse()
            {
                Role = Business.Administration.RoleGet(role)
            });
        }

        /// <summary>
        /// Add role 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("role.api")]
        public IActionResult AddRole(RoleAddRequest request)
        {
            Business.Administration.RoleAdd(request.Name);
            return GetOutput(201);
        }

        /// <summary>
        /// Updates role 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPatch("role.api")]
        public IActionResult UpdateRole(RoleUpdateRequest request)
        {
            Business.Administration.RoleUpdate(request.RoleId, request.Name);

            return GetOutput();
        }

        /// <summary>
        /// Deletes a role 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("role.api")]
        public IActionResult DeleteRole(Guid id)
        {
            Business.Administration.RoleDelete(id);

            return GetOutput();
        }

        /// <summary>
        /// Add resource to role 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("role.resource.api")]
        public IActionResult AddResourceRole(ResourceRoleRequest request)
        {
            Business.Administration.RoleAddResource(request.RoleId, request.ResourceId);

            return GetOutput();
        }

        /// <summary>
        /// Removes resource from role 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("role.resource.api")]
        public IActionResult DeleteResourceRole(ResourceRoleRequest request)
        {
            Business.Administration.RoleRemoveResource(request.RoleId, request.ResourceId);

            return GetOutput();
        }
        #endregion

        #region Language

        /// <summary>
        /// Adds language 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("language.api")]
        public IActionResult AddLanguage(LanguageRequest request)
        {
            Business.Administration.LanguageAdd(new Entities.Language() { Id = request.Id, Name = request.Name });
            return GetOutput();
        }

        /// <summary>
        /// Updates language 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPatch("language.api")]
        public IActionResult UpdateLanguage(LanguageRequest request)
        {
            Business.Administration.LanguageUpdate(new Entities.Language() { Id = request.Id, Name = request.Name });
            return GetOutput();
        }

        /// <summary>
        /// Deletes language 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("language.api")]
        public IActionResult UpdateLanguage(string id)
        {
            Business.Administration.LanguageDelete(id);
            return GetOutput();
        }

        #endregion

        #region Translation
        /// <summary>
        /// Add, Updates or deletes a translation. If exists the original for the specified language it will be updated. If not exists will be created. If the request.Translated value is null or empty will be deleted. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("translation.api")]
        public IActionResult TranslationSet(TranslationRequest request)
        {
            Business.Administration.TranslationSet(new Entities.Translation() { LanguageId = request.LanguageId, Original = request.Original, Translated = request.Translated });
            return GetOutput();
        }

        #endregion

        #region Resource
        /// <summary>
        /// Available resources 
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResourcesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("resources.api")]
        public IActionResult Resources()
        {
            return GetOutput(new ResourcesResponse()
            {
                Resources = Business.Administration.ResourceList()
            });
        }

        /// <summary>
        /// Add a resource 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("resource.api")]
        public IActionResult AddAssembly(ResourceAddRequest request)
        {
            Business.Administration.ResourceAdd(new Entities.Resource()
            {
                IsPublic = request.IsPublic,
                Label = request.Label,
                Method = request.Method,
                Order = request.Order,
                ParentId = request.ParentId,
                Title = request.Title,
                Url = request.Url
            });

            return GetOutput();
        }

        /// <summary>
        /// Updates a resource 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPatch("resource.api")]
        public IActionResult UpdateResource(ResourceUpdateRequest request)
        {
            Business.Administration.ResourceUpdate(new Entities.Resource()
            {
                Id = request.Id,
                IsPublic = request.IsPublic,
                Label = request.Label,
                Method = request.Method,
                Order = request.Order,
                ParentId = request.ParentId,
                Title = request.Title,
                Url = request.Url
            });

            return GetOutput();
        }

        /// <summary>
        /// Delete a resource 
        /// </summary>
        /// <param name="id">Resource id</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("resource.api")]
        public IActionResult DeleteResource(Guid id)
        {
            Business.Administration.ResourceDelete(id);
            return GetOutput();
        }
        #endregion

        #region User
        /// <summary>
        /// Disable a user 
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPatch("user.disable.api")]
        public IActionResult DisableUser(Guid id)
        {
            Business.Administration.UserDisable(id);
            return GetOutput();
        }

        /// <summary>
        /// Enable a user 
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPatch("user.enable.api")]
        public IActionResult EnableUser(Guid id)
        {
            Business.Administration.UserEnable(id);
            return GetOutput();
        }

        /// <summary>
        /// Enable a user 
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("user.api")]
        public IActionResult DeleteUser(Guid id)
        {
            Business.Administration.UserDelete(id);
            return GetOutput();
        }

        /// <summary>
        /// Authorizes user on a role 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("user.authorize.api")]
        public IActionResult UserAuthorize(UserAuthorizeRequest request)
        {
            Business.Administration.UserAuthorize(request.UserId, request.RoleId);

            return GetOutput();
        }

        /// <summary>
        /// Unauthorizes user on a role 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpDelete("user.unauthorize.api")]
        public IActionResult UserUnauthorize(UserAuthorizeRequest request)
        {
            Business.Administration.UserUnauthorize(request.UserId, request.RoleId);

            return GetOutput();
        }

        #endregion
    }
}
