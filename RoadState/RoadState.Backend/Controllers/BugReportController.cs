using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoadState.BusinessLayer;
using RoadState.BusinessLayer.TransportModels;
using RoadState.Data;
using RoadState.DataAccessLayer;
using System;
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
        private readonly ICommentCreator commentCreator;
        private readonly IMapper _mapper;
        public BugReportController(IBugReportFinder bugReportFinder, 
            IBugReportRater bugReportRater, 
            IMapper mapper, 
            IUserFinder userFinder,
            ICommentCreator commentCreator)
        {
            this.userFinder = userFinder;
            this.bugReportFinder = bugReportFinder;
            this.bugReportRater = bugReportRater;
            this._mapper = mapper;
            this.commentCreator = commentCreator;
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
        [HttpPost("/{id}/rate")]
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

        [HttpPost("{bugReportId}/comment")]
        public async Task<IActionResult> PostComment(int bugReportId, [FromBody] dynamic comment)
        {
            var commentDto = new CommentDto()
            {
                AuthorName = comment.authorName,
                Dislikes = comment.dislikes,
                Likes = comment.likes,
                PublishDate = Convert.ToDateTime(comment.publishDate),
                Text = comment.text
            };
            if (commentDto.AuthorName is null)
            {
                commentDto.AuthorName = "RoadStateGuest";
            }
            if ((await bugReportFinder.GetBugReportsAsync(x => x.Id == bugReportId)).Count == 0)
            {
                return NotFound("No such bug report");
            }
            if ((await userFinder.GetUsersAsync(x=> x.UserName == commentDto.AuthorName)).Count == 0)
            {
                return NotFound("No such users");
            }
            string userId = (await userFinder.GetUsersAsync(x => x.UserName == commentDto.AuthorName)).FirstOrDefault().Id;
            await commentCreator.CreateCommentAsync(new Comment()
            {
                AuthorId = userId,
                BugReportId = bugReportId,
                PublishDate = commentDto.PublishDate,
                Text = commentDto.Text
            });
            return NoContent();
        }
    }
}