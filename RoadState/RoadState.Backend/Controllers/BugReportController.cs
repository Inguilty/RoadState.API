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
        public async Task<IActionResult> GetBugReports([FromQuery] double longitudeMin, double longitudeMax, double latitudeMin, double latitudeMax)
        {
            var bugReports = _context.BugReports.Where(x => BugReportRectanglePredicate(x, longitudeMin, longitudeMax, latitudeMin, latitudeMax)).ToList();
            if (bugReports.Count == 0) return NotFound();
            return Ok(bugReports);
        }

        private bool BugReportRectanglePredicate(BugReport bugReport, double longitudeMin, double longitudeMax, double latitudeMin, double latitudeMax)
        {
            return bugReport.Location.Longitude >= longitudeMin && bugReport.Location.Longitude <= longitudeMax && bugReport.Location.Latitude >= latitudeMin && bugReport.Location.Latitude <= latitudeMax;
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