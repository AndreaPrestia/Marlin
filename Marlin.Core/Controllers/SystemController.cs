using Marlin.Core.Common;
using Marlin.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Marlin.Core.Controllers
{
    [Produces(ContentType.ApplicationJson)]
    [ApiController]
    [Route("system")]
    public class SystemController : SecureController
    {
        private static readonly NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        #region Access
        /// <summary>
        /// Signup
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("signup.api")]
        public IActionResult SignUp(SignUpRequest request)
        {
            Business.System.SignUp(request.Username);

            return GetOutput(201);
        }

        /// <summary>
        /// Login api
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("login.api")]
        public IActionResult Login(string username)
        {
            Business.Authorization.Login(username);

            return GetOutput();
        }

        /// <summary>
        /// Login api
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("login.api")]
        public IActionResult Login(LoginRequest request)
        {
            LoginResponse response = Business.Authorization.Login(request.Username, request.Password);

            return GetOutput(response);
        }

        /// <summary>
        /// Refresh marlin api bearer
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(RefreshResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("refresh.api")]
        public IActionResult Refresh()
        {
            string bearer = HttpContext.Request.Headers["Authorization"];

            if (String.IsNullOrWhiteSpace(bearer))
            {
                ThrowApplicationError("Bearer non found");
            }

            return GetOutput(new RefreshResponse() { Bearer = Business.Authorization.GetBearer(Context.Current.User) });
        }

        #endregion

        #region Languages
        /// <summary>
        /// Get language
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(LanguageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("language.api")]
        public IActionResult Language(string languageId)
        {
            return GetOutput(new LanguageResponse()
            {
                Language = Business.System.LanguageGet(languageId)
            });
        }
        #endregion

        #region User
        /// <summary>
        /// Current user info
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(InfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpGet("info.api")]
        public IActionResult Info()
        {
            return GetOutput(new InfoResponse()
            {
                User = Context.Current.User
            });
        }

        /// <summary>
        /// Set to the current user a property
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [HttpPost("property.api")]
        public IActionResult Property(PropertyRequest request)
        {
            Business.User.UserPropertySet(Context.Current.User, request.Key, request.Value);

            if (string.IsNullOrWhiteSpace(request.Value))
            {
                if (Context.Current.User.Properties.ContainsKey(request.Key))
                {
                    Context.Current.User.Properties.Remove(request.Key);
                }
            }
            else
            {
                if (Context.Current.User.Properties.ContainsKey(request.Key))
                {
                    Context.Current.User.Properties[request.Key] = request.Value;
                }
                else
                {
                    Context.Current.User.Properties.Add(request.Key, request.Value);
                }
            }

            return GetOutput();
        }

        /// <summary>
        /// Current user resources
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(InfoResponse), StatusCodes.Status200OK)]
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
                Resources = Business.Authorization.UserResources(Context.Current.User)
            });
        }

        #endregion
    }
}
