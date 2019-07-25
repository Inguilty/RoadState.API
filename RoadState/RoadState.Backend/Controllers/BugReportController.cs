using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadState.Data;
using RoadState.DataAccessLayer;
using AutoMapper;
using RoadState.BusinessLayer;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugReportController : ControllerBase
    {
        private readonly RoadStateContext _context;
        private readonly IMapper _mapper;
        public BugReportController(RoadStateContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetBugReports([FromQuery] double longitudeMin, double longitudeMax, double latitudeMin, double latitudeMax)
        {
            var bugReports = _context.BugReports.Where(x => BugReportRectanglePredicate(x, longitudeMin, longitudeMax, latitudeMin, latitudeMax)).ToList();
            if (bugReports.Count == 0) return NotFound();
            return Ok(bugReports);
        }

        private bool BugReportRectanglePredicate(BugReport bugReport, double longitudeMin, double longitudeMax, double latitudeMin, double latitudeMax)
        {
            return bugReport.Longitude >= longitudeMin && bugReport.Longitude <= longitudeMax && bugReport.Latitude >= latitudeMin && bugReport.Latitude <= latitudeMax;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBugReport(int id)
        {
            var bugReport = await _context.BugReports.FindAsync(id);
            if (bugReport is null) return NotFound();
            return Ok(_mapper.Map<BugReportDTO>(bugReport));
        }

        [HttpPost("{id}/rate")]
        public async Task<IActionResult> RateBugReport(int id, string rate)
        {
            var bugReport = await _context.BugReports.FindAsync(id);
            if (bugReport == null) return NotFound();
            return Ok();
        }
    }
}