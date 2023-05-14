using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Configs;
using WebApi6;

namespace WebApi6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class WebApi6TestController : ControllerBase
    {
        private readonly IOptionsMonitor<HotReloadConfigs> _hotReloadConfigs;
        private readonly IOptionsMonitor<QuartzConfig> _quartzConfig;

        public WebApi6TestController(
            IOptionsMonitor<HotReloadConfigs> hotReloadConfigs,
            IOptionsMonitor<QuartzConfig> quartzConfig)
        {
            _hotReloadConfigs = hotReloadConfigs;
            _quartzConfig = quartzConfig;
        }

        [HttpGet("HealthCheck")]
        public IActionResult Get()
        {
            return Ok(true);
        }

        [HttpGet("Limit")]
        public IActionResult GetLimit()
        {
            var limit = _hotReloadConfigs.CurrentValue.Limit;
            return Ok(limit);
        }

        [HttpGet("Quartz")]
        public IActionResult GetQuartzConfig()
        {
            var quartConfig = _quartzConfig.CurrentValue;
            return Ok(quartConfig.Limit);
        }
    }
}
