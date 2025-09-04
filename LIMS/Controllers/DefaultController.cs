using Microsoft.AspNetCore.Mvc;

namespace LIMS.Controllers
{
    [Route("/")]
    [ApiController]
    public class DefaultController:BaseApiController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                service = "LIMS ANDROID API",
                environment = "Production",
                status = "Running",
                serverTime = DateTime.UtcNow,
            });
        }
    }
}
