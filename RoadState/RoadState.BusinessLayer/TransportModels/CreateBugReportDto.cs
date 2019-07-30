using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadState.BusinessLayer.TransportModels
{
    public class CreateBugReportDto
    {
        public string ProblemLevel { get; set; }
        public string Description { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string UserId { get; set; }

    }
}
