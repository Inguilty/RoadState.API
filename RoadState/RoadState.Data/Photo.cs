using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.Data
{
    public class Photo : BaseEntity
    {
        public byte[] Blob { get; set; }
        public int BugReportId { get; set; }
        public BugReport BugReport { get; set; }
    }
}
