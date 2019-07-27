using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoadState.BusinessLayer;
using RoadState.Data;
using RoadState.DataAccessLayer;
using System.Linq;
using System.Threading.Tasks;
using RoadState.BusinessLayer.TransportModels;

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
        public async Task<IActionResult> GetBugReportsAsync([FromQuery] double longitudeMin, double longitudeMax, double latitudeMin, double latitudeMax)
        {
            var bugReports = await bugReportFinder.GetBugReportsAsync(x => BugReportRectanglePredicate(x, longitudeMin, longitudeMax, latitudeMin, latitudeMax));
            if (bugReports.Count == 0) return NotFound("no bug reports in this square");
            return Ok(bugReports);
        }

        private bool BugReportRectanglePredicate(BugReport bugReport, double longitudeMin, double longitudeMax, double latitudeMin, double latitudeMax)
        {
            return bugReport.Longitude >= longitudeMin && bugReport.Longitude <= longitudeMax && bugReport.Latitude >= latitudeMin && bugReport.Latitude <= latitudeMax;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBugReportAsync(int id)
        {

            var bugReports = await bugReportFinder.GetBugReportsAsync(x => x.Id == id);
            var bugReport = bugReports.FirstOrDefault();
            if (bugReport is null) return NotFound("No bug report found");
            return Ok(_mapper.Map<BugReportDto>(bugReport));
        }

        [HttpPost("{id}/rate")]
        public async Task<IActionResult> RateBugReportAsync(int id, string rate)
        {
            var bugReports = await bugReportFinder.GetBugReportsAsync(x => x.Id == id);
            var bugReport = bugReports.FirstOrDefault();
            if (bugReport is null) return NotFound("No bug report found");
            return Ok();
        }
    }
}