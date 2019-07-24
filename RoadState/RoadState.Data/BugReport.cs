using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.Data
{
    public class BugReport : BaseEntity
    {
        public User Author { get; set; }
        public Location Location { get; set; }
        public int Rating { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string UserRate { get; set; }
    }
}
