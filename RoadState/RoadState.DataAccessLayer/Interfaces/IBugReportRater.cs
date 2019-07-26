using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.DataAccessLayer.Interfaces
{
    public interface IBugReportRater
    {
        void RateBugReport(BugReport bugReport, User user, bool hasAgreed);
    }
}
