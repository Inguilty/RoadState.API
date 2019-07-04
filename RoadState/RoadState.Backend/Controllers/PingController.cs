﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoadState.Backend.Controllers
{
    [Route("api/ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [Route("ping1")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}