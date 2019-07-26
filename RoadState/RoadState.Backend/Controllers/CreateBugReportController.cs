using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoadState.BusinessLayer;

namespace RoadState.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateBugReportController : ControllerBase
    {
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

            if (Request.Form.Files.Count > 0)
            {
                foreach (var file in Request.Form.Files)
                {
                    if(file.Name == "Data")
                    {
                        string allText = "";
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            allText = reader.ReadToEnd();
                        }
                        if(allText == "" || allText == null)
                        {
                            return BadRequest();
                        }
                        var jsonObject = JsonConvert.DeserializeObject<CreateBugReportDTO>(allText);
                        
                    }
                }
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


            return Ok();
    }
}
}