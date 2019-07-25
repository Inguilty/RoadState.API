using System;
using System.Collections.Generic;

namespace RoadState.Data
{
    public class BugReport : BaseEntity
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Rating { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public List<BugReportRate> BugReportRates { get; set; } = new List<BugReportRate>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
