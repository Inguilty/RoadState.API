using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoadState.BusinessLayer;
using RoadState.Data;
using RoadState.DataAccessLayer.Interfaces;
using System.Linq;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugReportController : ControllerBase
    {
        private readonly IBugReportFinder bugReportFinder;
        private readonly IBugReportRater bugReportRater;
        private readonly IMapper _mapper;
        public BugReportController(IBugReportFinder bugReportFinder, IBugReportRater bugReportRater, IMapper mapper)
        {
            this.bugReportFinder = bugReportFinder;
            this.bugReportRater = bugReportRater;
            this._mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetBugReports([FromQuery] double longitudeMin, double longitudeMax, double latitudeMin, double latitudeMax)
        {
            var bugReports = bugReportFinder.GetBugReports(x => BugReportRectanglePredicate(x, longitudeMin, longitudeMax, latitudeMin, latitudeMax)).ToList();
            if (bugReports.Count == 0) return NotFound();
            return Ok(bugReports);
        }

        private bool BugReportRectanglePredicate(BugReport bugReport, double longitudeMin, double longitudeMax, double latitudeMin, double latitudeMax)
        {
            return bugReport.Longitude >= longitudeMin && bugReport.Longitude <= longitudeMax && bugReport.Latitude >= latitudeMin && bugReport.Latitude <= latitudeMax;
        }

        [HttpGet("{id}")]
        public IActionResult GetBugReport(int id)
        {

            var bugReport = bugReportFinder.GetBugReports(x => x.Id == id).Single();
            if (bugReport is null) return NotFound();
            return Ok(_mapper.Map<BugReportDto>(bugReport));
        }

        [HttpPost("{id}/rate")]
        public IActionResult RateBugReport(int id, string rate)
        {
            var bugReport = bugReportFinder.GetBugReports(x => x.Id == id).Single();
            if (bugReport == null) return NotFound();
            return Ok();
        }
    }
}