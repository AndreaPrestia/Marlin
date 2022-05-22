using Marlin.Core;
using Marlin.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marlin.Web.Api
{
    public class ExampleHandler : ApiHandler
    {
        private readonly TokenManager _tokenManager;

        public ExampleHandler(TokenManager tokenManager)
        {
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        [ApiRoute("/free", "GET")]
        public ApiOutput Free()
        {
            return Ok("I'm freeeee");
        }

        [ApiRoute("/token", "GET")]
        public ApiOutput Token([ApiParameter] string username, [ApiParameter] string secret)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("No username provided");
            }

            if (string.IsNullOrEmpty(secret))
            {
                return BadRequest("No secret provided");
            }

            if(!username.Equals("Silvio") && !secret.Equals("Berlusconi"))
            {
                return Forbidden("Invalid credentials");
            }

            Context.Add("username", "Silvio");
            Context.Add("roles", "guests");

            return Ok(_tokenManager.Jwt());
        }

        [ApiRoute("/secured", "GET")]
        [Secured("roles", "users, guests")]
        public ApiOutput Secured()
        {
            return Ok("I'm secured!");
        }
    }
}
