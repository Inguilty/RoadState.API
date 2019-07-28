using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoadState.BusinessLayer;
using RoadState.Data;
using RoadState.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugReportController : ControllerBase
    {
        private readonly IBugReportFinder bugReportFinder;
        private readonly IBugReportRater bugReportRater;
        private readonly IBugReportCreator bugReportCreator;
        private readonly IMapper _mapper;
        public BugReportController(IBugReportFinder bugReportFinder, IBugReportRater bugReportRater, IMapper mapper, IBugReportCreator bugReportCreator)
        {
            this.bugReportFinder = bugReportFinder;
            this.bugReportRater = bugReportRater;
            this.bugReportCreator = bugReportCreator;
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

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> CreateBugReportAsync()
        {
            if (Request.Form.Files.Count > 1)
            {
                CreateBugReportDto createBR = new CreateBugReportDto();
                List<byte[]> photos = new List<byte[]>();
                foreach (var file in Request.Form.Files)
                {
                    if (file.Name == "Data")
                    {
                        string allText = "";
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            allText = reader.ReadToEnd();
                        }
                        if (allText == "" || allText == null)
                        {
                            return BadRequest();
                        }
                        createBR = JsonConvert.DeserializeObject<CreateBugReportDto>(allText);

                    }
                    else
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            photos.Add(ms.ToArray());
                        }
                    }
                }
                var newBR = _mapper.Map<BugReport>(createBR);
                newBR.Photos = _mapper.Map<List<Photo>>(photos);
                newBR.PublishDate = DateTime.Now;
                await bugReportCreator.CreateBugReportAsync(newBR);
            }
            else
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}