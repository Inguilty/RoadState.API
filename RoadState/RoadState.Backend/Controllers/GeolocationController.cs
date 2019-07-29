using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeolocationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public GeolocationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private async Task<string> GetAddress(double latitude, double longitude)
        {
            string publicKey = _configuration["AppSettings:GoogleMapsAPIKey"];
            string BASE_URL = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={publicKey}";
            var client = new HttpClient();
            var response = await client.GetAsync(BASE_URL);
            if (response.StatusCode != HttpStatusCode.OK) return null;
            return await response.Content.ReadAsStringAsync();
        }

        [HttpGet("coords")]
        public async Task<IActionResult> GetCurrentAddress([FromQuery] double longitude, double latitude)
        {
            string result = await GetAddress(latitude, longitude);
            if (result is null) return Forbid("No access to Google Maps API");
            return Ok(result);
        }
    }
}