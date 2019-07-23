using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadState.Backend.Data;
using RoadState.Backend.Models;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugReportController : ControllerBase
    {
        public MockContext _context = new MockContext();

        [HttpGet]
        public async Task<IActionResult> GetBugReports()
        {
            return Ok(_context.BugReports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBugReport(int id)
        {
            var bugReport = _context.BugReports.Find(x => x.Id == id);
            if (bugReport is null) return NotFound();
            return Ok(bugReport);
        }
    }
}