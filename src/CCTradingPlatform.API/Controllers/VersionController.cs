using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CCTradingPlatform.API.Controllers
{
    [ApiController]
    [Route("api/version")]
    public class VersionController : ControllerBase
    {
        private readonly ILogger<VersionController> _logger;

        public VersionController(ILogger<VersionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetVersion()
        {
            return Ok(new { version = "v1.0" });
        }
    }
}
