using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoadState.BusinessLayer;
using RoadState.BusinessLayer.TransportModels;
using RoadState.Data;
using RoadState.DataAccessLayer;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Geocoding;
using Geocoding.Google;
using RoadState.Backend.Helpers;
using Microsoft.Extensions.Options;

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
        private readonly ICommentCreator commentCreator;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        public BugReportController(IBugReportFinder bugReportFinder, 
            IBugReportRater bugReportRater, 
            IMapper mapper, 
            IUserFinder userFinder,
            ICommentCreator commentCreator,
            IBugReportCreator bugReportCreator,
            IOptions<AppSettings> appSettings
            )
        {
            this.userFinder = userFinder;
            this.bugReportFinder = bugReportFinder;
            this.bugReportRater = bugReportRater;
            this.bugReportCreator = bugReportCreator;
            this._mapper = mapper;
            this.commentCreator = commentCreator;
            this._appSettings = appSettings.Value;
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
        public async Task<IActionResult> GetBugReportAsync(int id, string userId)
        {

            var bugReports = await bugReportFinder.GetBugReportsAsync(x => x.Id == id);
            var bugReport = bugReports.FirstOrDefault();
            if (bugReport is null) return NotFound("No bug report found");
            var hasUserRated = bugReport.BugReportRates.Count(x => x.UserId == userId) != 0;
            var mapped = _mapper.Map<BugReportDto>(bugReport);
            mapped.UserRate = hasUserRated ? bugReport.BugReportRates.FirstOrDefault(x => x.UserId == userId).HasAgreed ? "agree" : "disagree" : null;
            return Ok(mapped);
        }

        [Authorize]
        [HttpPost("{id}/rate")]
        public async Task<IActionResult> RateBugReportAsync([FromBody] UserRateDTO userRateDTO)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(userRateDTO.Token) as JwtSecurityToken;
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var bugReport = (await bugReportFinder.GetBugReportsAsync(x => x.Id == userRateDTO.Id)).FirstOrDefault();
            if (bugReport is null) return NotFound("No bug report found");
            var user = (await userFinder.GetUsersAsync(x => x.Id == userId)).FirstOrDefault();
            bool hasAgreed = userRateDTO.Rate == "agree";
            if (hasAgreed)
            {
                bugReport.Rating++;
            }
            else
            {
                bugReport.Rating--;
            }
            await bugReportRater.RateBugReportAsync(bugReport, user, hasAgreed);
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
                        if (String.IsNullOrEmpty(allText))
                        {
                            return BadRequest("No data provided");
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
                if(user == null)
                {
                    return BadRequest("No user provided");
                }
                newBR.Author = user;
                await bugReportCreator.CreateBugReportAsync(newBR);
            }
            else
            {
                return BadRequest("Not enough data provided");
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

        [HttpGet("address")]
        public async Task<IActionResult> GetAddressByCoordinatesAsync(string latitude, string longitude)
        {
            IGeocoder geocoder = new GoogleGeocoder(_appSettings.ApiKey);
            if (String.IsNullOrEmpty(longitude) || String.IsNullOrEmpty(latitude))
            {
                return BadRequest("No coordinates provided");
            }
            IEnumerable<Address> addresses = await geocoder.ReverseGeocodeAsync(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
            var addresse = addresses.FirstOrDefault();
            if (addresse is null) return NotFound("No addresses found");
            return Ok(addresse.FormattedAddress);
        }
    }
}