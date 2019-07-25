using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.Data
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public List<BugReportRate> BugReportRates { get; set; } = new List<BugReportRate>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<UserLikes> UserLikes { get; set; } = new List<UserLikes>();
    }
}
