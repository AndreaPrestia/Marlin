using Marlin.Core;

namespace Marlin.Web.Api
{
    public class Secured : ApiHandler
    {
        public override ApiOutput Process(ApiInput input)
        {
            return Ok("I'm secured");
        }
    }
}