using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateBugReportController : Controller
    {
        [HttpPost()]
        public async Task<IActionResult> CreateBugReport()
            [FromBody] 
        {

            return Ok(_context.BugReports);
        }
    }
}