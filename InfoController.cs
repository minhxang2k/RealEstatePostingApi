using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace RealEstatePostingApi
{
    public class InfoController : ControllerBase
    {
        //private readonly ILogger<InfoController> _logger;
        private readonly AppOptions _appOptions;
        public InfoController( IOptions<AppOptions> appOptions)
        {
            //_logger = logger;
            _appOptions = appOptions.Value;
        }
        [HttpGet]
        [Route("info")]
        public IActionResult GetInfo()
        {
            var info = new
            {
                ServiceName = _appOptions.Servicename,
                Environment = _appOptions.EnvironmentLabel,
                Timestamp = DateTime.UtcNow
            };
            return Ok(info);
        }
    }
}
