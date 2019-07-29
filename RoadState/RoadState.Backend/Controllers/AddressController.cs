using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geocoding;
using Geocoding.Google;
using RoadState.Backend.Helpers;
using Microsoft.Extensions.Options;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        public AddressController(
            IOptions<AppSettings> appSettings
            )
        {
            this._appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressByCoordinatesAsync(string latitude, string longitude)
        {
            IGeocoder geocoder = new GoogleGeocoder(_appSettings.ApiKey);
            if (String.IsNullOrEmpty(longitude) || String.IsNullOrEmpty(latitude))
            {
                return BadRequest("No coordinates provided");
            }
            IEnumerable<Address> addresses = await geocoder.ReverseGeocodeAsync(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
            var addresse = addresses.FirstOrDefault();
            if (addresse is null) return NotFound("No addresses found");
            return Ok(addresse.FormattedAddress);
        }
    }
}