using RoadState.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                ReliabilityLevel = 1,
                ProblemLevel = 1,
                Description = "Test",
                PublishDate = DateTime.Now
            }
        };
    }
}
