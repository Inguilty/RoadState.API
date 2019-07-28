using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RoadState.BusinessLayer.Services
{
    public interface IBugReportCreator
    {
        Task<bool> AddBugReportAsync(IFormFileCollection bugReportFiles);
    }
   
    class BugReportService : IBugReportCreator
    {
        public async Task<bool> AddBugReportAsync(IFormFileCollection bugReportFiles)
        {
            CreateBugReportDto createBR = new CreateBugReportDto();
            List<byte[]> photos = new List<byte[]>();
            foreach (var file in bugReportFiles)
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
                        return false;
                    }
                    var jsonObject = JsonConvert.DeserializeObject<CreateBugReportDto>(allText);

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
            return true;
        }
    }
}
