using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoadState.BusinessLayer;
using RoadState.Data;
using RoadState.DataAccessLayer;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateBugReportController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBugReportCreator bugReportCreator;

        public CreateBugReportController(IBugReportCreator bugReportCreator, IMapper mapper)
        {
            this.bugReportCreator = bugReportCreator;
            this._mapper = mapper;
        }
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> CreateBugReport()
            //[ModelBinder(BinderType = typeof(JsonModelBinder))] CreateBugReportDTO Data)
            //[FromBody] CreateBugReportDTO createBugReport)
        {
            
            /*if (createBugReport == null)
            {
                return BadRequest();
            }*/

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
                await bugReportCreator.CreateBugReportAsync(newBR);
                //Loop through uploaded files  
                //for (int i = 0; i < httpContext.Request.Form.Files.Count; i++)
                //{
                //    HttpPostedFile httpPostedFile = httpContext.Request.Files[i];
                //    if (httpPostedFile != null)
                //    {
                //        // Construct file save path  
                //        var fileSavePath = Path.Combine(HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadFolder"]), httpPostedFile.FileName);

                //        // Save the uploaded file  
                //        httpPostedFile.SaveAs(fileSavePath);
                //    }
                //}
            }
            else
            {
                return BadRequest();
            }


            return Ok();
    }
}
}