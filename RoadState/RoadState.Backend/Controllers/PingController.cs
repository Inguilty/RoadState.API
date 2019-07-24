using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoadState.Backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PingController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [Route("ping")]
        public IActionResult Get()
        {
            return Ok(new { status = "OK" });
        }
    }
}