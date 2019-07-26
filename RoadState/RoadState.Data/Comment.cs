using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.Data
{
    public class Comment: BaseEntity
    {
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public int BugReportId { get; set; }
        public BugReport BugReport { get; set; }
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }

        public List<UserMark> UserLikes { get; set; } = new List<UserMark>();
    }
}
