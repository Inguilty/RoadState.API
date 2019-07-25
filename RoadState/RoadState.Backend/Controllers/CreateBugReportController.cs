using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoadState.BusinessLayer;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateBugReportController : ControllerBase
    {
        [HttpPost()]
        public async Task<IActionResult> CreateBugReport(
            [FromForm] CreateBugReportDTO createBugReport)
        {
            
            if (createBugReport == null)
            {
                return BadRequest();
            }

            if (Request.Form.Files.Count > 0)
            {
                foreach (var photo in Request.Form.Files)
                {

                }
                //Loop through uploaded files  
                for (int i = 0; i < httpContext.Request.Form.Files.Count; i++)
                {
                    HttpPostedFile httpPostedFile = httpContext.Request.Files[i];
                    if (httpPostedFile != null)
                    {
                        // Construct file save path  
                        var fileSavePath = Path.Combine(HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadFolder"]), httpPostedFile.FileName);

                        // Save the uploaded file  
                        httpPostedFile.SaveAs(fileSavePath);
                    }
                }
            }


            return Ok();
    }
}
}