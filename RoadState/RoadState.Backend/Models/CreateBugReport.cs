using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadState.Backend.Models
{
    public class CreateBugReport
    {
        
        public Location Location { get; set; }
        public int ReliabilityLevel { get; set; }
        public int ProblemLevel { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
