using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadState.BusinessLayer
{
    public class CreateBugReportDTO
    {
        public int ProblemLevel { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        
        
    }
}
