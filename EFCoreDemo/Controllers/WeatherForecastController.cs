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

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOptions<JwtSettings> jwtSettings;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IOptions<JwtSettings> jwtSettings)
        {
            _logger = logger;
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
            return Ok(jwtSettings.Value);
        }
    }
}