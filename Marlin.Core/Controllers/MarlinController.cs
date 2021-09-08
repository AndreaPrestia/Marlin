using Microsoft.AspNetCore.Mvc;

namespace Marlin.Core.Controllers
{
    public class MarlinController : ControllerBase
    {
        public IActionResult Index()
        {
            return new OkObjectResult("Marlin is up and running :)");
        }
    }
}
