using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoadState.BusinessLayer;
using RoadState.BusinessLayer.TransportModels;
using RoadState.Data;
using RoadState.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugReportController : ControllerBase
    {
        private readonly IUserFinder userFinder;
        private readonly IBugReportFinder bugReportFinder;
        private readonly IBugReportRater bugReportRater;
        private readonly IBugReportCreator bugReportCreator;
        private readonly IMapper _mapper;
        public BugReportController(IBugReportFinder bugReportFinder, IBugReportRater bugReportRater, IMapper mapper, IUserFinder userFinder, IBugReportCreator bugReportCreator)
        {
            this.userFinder = userFinder;
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
            return Ok(bugReports.Select(x=>_mapper.Map<BugReportDto>(x)));
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

        [Authorize]
        [HttpPost("{id}/rate")]
        public async Task<IActionResult> RateBugReportAsync(int id, string rate)
        {
            var bugReport = (await bugReportFinder.GetBugReportsAsync(x => x.Id == id)).FirstOrDefault();
            if (bugReport is null) return NotFound("No bug report found");
            var user = (await userFinder.GetUsersAsync(x => x.Id == User.Identity.Name)).FirstOrDefault();
            bool hasAgreed = rate == "agree";
            await bugReportRater.RateBugReportAsync(bugReport, user, hasAgreed);
            if(hasAgreed)
            {
                bugReport.Rating++;
            }
            else
            {
                bugReport.Rating--;
            }
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
                var user = (await userFinder.GetUsersAsync(x => x.Id == newBR.AuthorId)).FirstOrDefault();
                if(user != null)
                {
                    newBR.Author = user;
                }
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