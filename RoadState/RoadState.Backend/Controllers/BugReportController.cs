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
using Microsoft.EntityFrameworkCore;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugReportController : ControllerBase
    {
        private readonly Repository _repository;
        private readonly IMapper _mapper;
        public BugReportController(Repository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetBugReports([FromQuery] double longitudeMin, double longitudeMax, double latitudeMin, double latitudeMax)
        {
            var bugReports = _repository.GetBugReports().Where(x => BugReportRectanglePredicate(x, longitudeMin, longitudeMax, latitudeMin, latitudeMax)).ToList();
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

            var bugReport = _repository.GetBugReports().FirstOrDefault(x => x.Id == id);
            var a = _repository.GetBugReports();
            if (bugReport is null) return NotFound();
            return Ok(_mapper.Map<BugReportDTO>(bugReport));
        }

        [HttpPost("{id}/rate")]
        public IActionResult RateBugReport(int id, string rate)
        {
            var bugReport = _repository.GetBugReport(id);
            if (bugReport == null) return NotFound();
            return Ok();
        }
    }
}