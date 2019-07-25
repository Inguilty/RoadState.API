using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.Data
{
    public class BugReportRate: BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int BugReportId { get; set; }
        public BugReport BugReport { get; set; }

        public bool HasAgreed { get; set; }
    }
}
