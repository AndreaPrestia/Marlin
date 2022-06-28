using Marlin.Core;

namespace Marlin.Web.Api
{
    public class Jwt : ApiHandler
    {
        public override ApiOutput Process(ApiInput input)
        {
            var username = input.GetQueryParameter<string>("u");
            
            var password = input.GetQueryParameter<string>("p");

            if(!username.Equals("Silvio") && !password.Equals("Berlusconi"))
            {
                return Forbidden("Invalid credentials");
            }

            Context.Add("username", "Silvio");
            Context.Add("roles", "guests");

            return Ok(Context.Current.Jwt);
        }
    }
}