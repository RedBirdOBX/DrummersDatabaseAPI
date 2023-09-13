using Microsoft.AspNetCore.Mvc;

namespace DrummersDatabaseAPI.Web.Controllers
{
    /// <summary>
    /// Health Check controller
    /// </summary>
    [Route("api/health")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {

        /// <summary>
        /// health check endpoint
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult HealthCheck()
        {
            return Ok("Failure is the opportunity to begin again more intelligently.");
        }
    }
}
