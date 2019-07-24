using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoadState.Data;

namespace RoadState.Backend.Data
{
    public class MockContext
    {
        public List<BugReport> BugReports { get; private set; } = new List<BugReport>()
        {
            new BugReport()
            {
                Id = 1,
                Author = new User()
                {
                    Email = "admin@gmail.com",
                    HomeLocation = null,
                    RegistrationDate = DateTime.Now,
                    UserName = "admin"
                },
                Location = null,
                Rating = 1,
                State = "Very low",
                Description = "Road has not been repaired for 20 years",
                PublishDate = DateTime.Now,
                UserRate = null
            }
        };
    }
}
