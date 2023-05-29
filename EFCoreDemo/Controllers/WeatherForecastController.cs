using EFCoreDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EFCoreDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger _customLogger;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOptions<JwtSettings> jwtSettings;

        public WeatherForecastController(
            IWebHostEnvironment env,
            ILogger<WeatherForecastController> logger,
            ILoggerFactory loggerFactory,
            IOptions<JwtSettings> jwtSettings)
        {
            this.env = env;
            _logger = logger;
            _customLogger = loggerFactory.CreateLogger("CustomLogger.v1");
            this.jwtSettings = jwtSettings;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("~/test", Name = "GetWeatherForecastTest")]
        public IActionResult GetTest()
        {
            return Ok(new
            {
                Name = User.Identity?.Name,
                IsAuthenticated = User.Identity?.IsAuthenticated
            });
        }

        [HttpGet("~/jwt")]
        [AllowAnonymous]
        public IActionResult GetJwtSettings()
        {
            if (env.IsEnvironment("UAT"))
            {
                _logger.LogTrace("GetJwtSettings on UAT");
                _logger.LogDebug("GetJwtSettings on UAT");
            }

            _logger.LogInformation(
                "GetJwtSettings jwtSettings.Value.Issuer={Issuer} " +
                "jwtSettings.Value.SignKey={SignKey}",
                jwtSettings.Value.Issuer,
                jwtSettings.Value.SignKey);
            _logger.LogWarning("GetJwtSettings");
            _logger.LogError("GetJwtSettings");
            _logger.LogCritical("GetJwtSettings");

            _customLogger.LogInformation("customLogger => GetJwtSettings");

            return Ok(jwtSettings.Value);
        }

        [HttpGet("~/rid")]
        [AllowAnonymous]
        public IActionResult GetRequestId(int err = 0)
        {
            _logger.LogInformation(new EventId(999), "GetRequestId");

            if (err == 1)
            {
                _logger.LogError(new ArgumentException("BAD"), "GetRequestId");
            }

            return Ok(HttpContext.TraceIdentifier);
        }
    }
}