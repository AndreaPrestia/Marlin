using Marlin.Core;

namespace Marlin.Web.Api
{
    public class Free : ApiHandler
    {
        public override ApiOutput Process(ApiInput input)
        {
            return Ok("I'm free");
        }
    }
}